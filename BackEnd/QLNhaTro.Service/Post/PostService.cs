using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QLNhaTro.Commons;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Moddel.Moddel.ResponseModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonConstants;

namespace QLNhaTro.Service.Post
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _Context;

        public PostService(AppDbContext appDbContext)
        {
            _Context = appDbContext;
        }
        public GetDetailPostResModel GetDetail(long Id)
        {
            var post = _Context.SharedResidents.GetAvailableById(Id);
            var result = new GetDetailPostResModel
            {
                Id = post.Id,
                Name = post.Name,   
                Address = post.Address, 
                PriceRoom = post.PriceRoom,
                Gender = post.Gender,
                Describe = post.Describe,
                PathImgRoom = ConverPathListIMG(_Context.NewRoomPhotos.Where(item=> item.NewId == post.Id).Select(record=> record.Path).ToList()),
            };
            return result;
        }
        public List<GetAllPostByCustomerResModel> GetByCustomer(long customerId)
        {
            var post = _Context.SharedResidents.Where(item=> item.CustomerId == customerId && !item.IsDeleted).Select(record=> new GetAllPostByCustomerResModel
            {
                Id = record.Id,
                Address = record.Address,
                Gender= record.Gender == 1 ? "Nam" : "Nữ",
                Name = record.Name,
                Price = record.PriceRoom,
                Status = record.IsFound
            }).ToList();
            foreach (var item in post)
            {
                item.PathImgRoom = ConverPathListIMG(_Context.NewRoomPhotos.Where(i=>i.NewId == item.Id).Select(i=> i.Path).ToList());
            }
            return post;
        }
        private static List<string> ConverPathListIMG(List<string> input)
        {
            List<string> result = new List<string>();
            foreach (var item in input)
            {
                result.Add(CommonFunctions.ConverPathIMG(item));
            }
            return result;
        }
        public async Task CreateEdit(CreateEditPostReqModel input,List<IFormFile> ImgRoom)
        {
            if(input.Id <= 0)
            {
                var newPost = new SharedResidents
                {
                    Name = input.Name,
                    Address = input.Address,
                    PriceRoom = input.PriceRoom,
                    Gender = input.Gender,
                    Describe = input.Describe,
                    CustomerId = input.CustomerId,
                };
                _Context.SharedResidents.Add(newPost);
                await _Context.SaveChangesAsync();
                SaveImgToDB(ImgRoom, newPost.Id);
                await _Context.SaveChangesAsync();
            }
            else
            {
                var post = _Context.SharedResidents.GetAvailableById(input.Id);
                post.Name = input.Name;
                post.Address = input.Address;
                post.PriceRoom = input.PriceRoom;
                post.Gender = input.Gender;
                post.Describe = input.Describe;
                string path = @$"{DefaultValue.DEFAULT_BASE_Directory_IMG}\images\Post\{post.Id}";
                DeleteImg(path, post.Id);
                SaveImgToDB(ImgRoom, input.Id);
                _Context.SharedResidents.Update(post);
                await _Context.SaveChangesAsync();
            }
        }
        private void SaveImgToDB(List<IFormFile> Imgs, long PostId)
        {
            try
            {
                // Đường dẫn thư mục lưu ảnh
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $@"{DefaultValue.DEFAULT_BASE_Directory_IMG}\images\Post\{PostId}");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                foreach (var img in Imgs)
                {
                    if (img.Length > 0)
                    {
                        // Đặt tên file duy nhất
                        var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);

                        // Đường dẫn đầy đủ của ảnh
                        var filePath = Path.Combine(directoryPath, fileName);

                        // Lưu file vào đường dẫn đã chỉ định
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            img.CopyTo(stream);
                        }

                        var imgRoom = new NewRoomPhoto { NewId = PostId, Path = filePath };
                        _Context.NewRoomPhotos.Add(imgRoom);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        private void DeleteImg(string path, long newId)
        {
            try
            {
                // Kiểm tra xem thư mục có tồn tại không
                if (Directory.Exists(path))
                {
                    // Lấy tất cả các file trong thư mục
                    var files = Directory.GetFiles(path);

                    Directory.Delete(path, true);
                    Console.WriteLine("Đã xóa thư mục và tất cả nội dung trong: " + path);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            var imgRoom = _Context.NewRoomPhotos.Where(record => record.NewId == newId).ToList();
            _Context.NewRoomPhotos.RemoveRange(imgRoom);
        }
        public async Task DetelePost(long Id)
        {
            var post = _Context.SharedResidents.GetAvailableById(Id);
            _Context.SharedResidents.Remove(post);
            var imgPost = _Context.NewRoomPhotos.Where(item => item.NewId == post.Id).ToList();
            _Context.NewRoomPhotos.RemoveRange(imgPost);
            try
            {
                string path = @$"{DefaultValue.DEFAULT_BASE_Directory_IMG}\images\Post\{post.Id}";
                if (Directory.Exists(path))
                {
                    // Xóa thư mục và tất cả các thư mục con
                    Directory.Delete(path, true);
                }
                else
                {
                    throw new Exception("Thử mục lưu ảnh không tồn tại");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            await _Context.SaveChangesAsync();
        }
        public async Task Repost(long postId)
        {
            var post = _Context.SharedResidents.GetAvailableById(postId);
            if(!post.IsFound)
            {
                throw new Exception("Bài đăng vẫn đang được tìm kiếm ");
            }
            post.IsFound = false;
            _Context.SharedResidents.Update(post);
            await _Context.SaveChangesAsync();
        }
        public async Task Found(long postId)
        {
            var post = _Context.SharedResidents.GetAvailableById(postId);
            if (post.IsFound)
            {
                throw new Exception("Bài đăng đã được đóng");
            }
            post.IsFound = true;
            _Context.SharedResidents.Update(post);
            await _Context.SaveChangesAsync();
        }
        public List<GetAllPostByFindPeopleResModel> FindPeople(string address, decimal priceForm, decimal priceArrive, long customerId,int genter)
        {
            var post = _Context.SharedResidents
                .Where(item => item.Address.Contains(address) && item.PriceRoom >= priceForm && item.PriceRoom <= priceArrive && !item.IsDeleted && !item.IsFound && (genter == 0 || item.Gender == genter))
                .Select(p => new GetAllPostByFindPeopleResModel
                {
                    Name = p.Name,
                    Address = p.Address,
                    Id = p.Id,
                    Price = p.PriceRoom,
                    Gender = p.Gender == 1 ? "Nam" : "Nữ",
                    
                }).ToList();
            if (post.Count == 0)
            {
                throw new Exception("Không có phòng nào phù hợp với điều kiện của bạn");
            }
            foreach (var item in post)
            {
                if (customerId != 0)
                {
                    item.IsSave = _Context.SaveNews.Any(s => s.CustomerId == customerId && s.NewId == item.Id);
                }
                item.Img = ConverPathListIMG(_Context.NewRoomPhotos.Where(r => r.NewId == item.Id).Select(record => record.Path).ToList());
            }
            return post;
        }
        public async Task SavePost(long customerId, long postId, bool status)
        {
            if (customerId == 0)
            {
                throw new Exception("Vui lòng đăng nhập trước");
            }
            if (status)
            {
                var save = _Context.SaveNews.Where(item => item.NewId == postId && item.CustomerId == customerId).FirstOrDefault();
                if (save == null)
                {
                    throw new Exception("Phòng chưa được lưu");
                }
                _Context.SaveNews.Remove(save);
                await _Context.SaveChangesAsync();

            }
            else
            {
                if (!_Context.Customers.Any(item => item.Id == customerId && !item.IsDeleted))
                {
                    throw new Exception("Người dùng không tồn tại");
                };
                if (!_Context.SharedResidents.Any(item => item.Id == postId && !item.IsDeleted))
                {
                    throw new Exception("Phòng không tồn tại");
                }
                if (_Context.SaveNews.Any(item => item.NewId == postId && item.CustomerId == customerId))
                {
                    throw new Exception("Phòng đã được lưu");
                }
                var newSavePost = new SaveNews
                {
                    CustomerId = customerId,
                    NewId = postId
                };
                _Context.SaveNews.Add(newSavePost);
                await _Context.SaveChangesAsync();
            }
        }
        public GetDetailPostByFindResModel GetDetailPostByFind(long Id, long customerId)
        {
            var postData = _Context.SharedResidents.Include(r => r.Customer)
                .Where(r => r.Id == Id && !r.IsDeleted && !r.IsFound)
                .Select(record => new GetDetailPostByFindResModel
                {
                    Id = record.Id,
                    Name = record.Name,
                    Address = record.Address,
                    PriceRoom = record.PriceRoom,
                    Gender = record.Gender == 1 ? "Nam" : "Nữ",
                    MoTa = record.Describe,
                    CustomerName = record.Customer.FullName,
                    SDTZalo = record.Customer.SDTZalo,
                    SDTCustomer = record.Customer.PhoneNumber,
                    CustomerAvata = CommonFunctions.ConverPathIMG(record.Customer.PathAvatar),
                    PathImgRoom = ConverPathListIMG(_Context.NewRoomPhotos.Where(item => item.NewId == record.Id).Select(i => i.Path).ToList()),
                }).FirstOrDefault();
            if (postData == null)
            {
                throw new Exception("Phòng không tồn tại");
            }
            postData.IsSave = _Context.SaveNews.Any(item => item.CustomerId == customerId && item.NewId == postData.Id);
            return postData;
        }
    }
}
