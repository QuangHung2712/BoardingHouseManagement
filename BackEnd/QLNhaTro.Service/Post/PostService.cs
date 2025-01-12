using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QLNhaTro.Commons;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
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
                Gender= record.Gender,
                Name = record.Name,
                Price = record.PriceRoom,
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
                DeleteImgRoom(path, post.Id);
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
        private void DeleteImgRoom(string path, long newId)
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
            await _Context.SaveChangesAsync();
        }
    }
}
