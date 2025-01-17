using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Moddel.Moddel.ResponseModels.Post;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonEnums;

namespace QLNhaTro.Service.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _Context;

        public CustomerService(AppDbContext context)
        {
            _Context = context;
        }
        public List<CustomerResModel> GetCustomerByContract(long contractId)
        {
            var customerData = _Context.ContractCustomers.Where(c=>c.ContractId == contractId && !c.Contract.IsDeleted).Select(c=> CustomerResModel.Mapping(c.Customer,c.IsRepresentative)).ToList();
            return customerData;
        }
        public string GetCustomerNameByContract(long contractId)
        {
            var customerData = _Context.ContractCustomers.Where(c => c.ContractId == contractId && !c.Contract.IsDeleted).Select(c => c.Customer.FullName).ToList();
            return string.Join(", ", customerData);
        }
        public string GetCustomerPhoneByContract(long contractId)
        {
            var customerData = _Context.ContractCustomers.Where(c => c.ContractId == contractId && !c.Contract.IsDeleted).Select(c => c.Customer.PhoneNumber).ToList();
            return string.Join(", ", customerData);
        }
        public async Task CreateCustomer(CreateEditCustomerReqModel input)
        {
            try
            {
                var result = new Customers
                {
                        
                    FullName = input.FullName,
                    DoB = input.DoB,
                    PhoneNumber = input.PhoneNumber,
                    Email = input.Email,
                    Address = input.Address,
                    CCCD = input.CCCD,
                    SDTZalo = input.SDTZalo,
                    PathAvatar = CommonConstants.DefaultValue.DEFAULT_IMG_AVATAR,
                    Password = CommonConstants.DefaultValue.DEFAULT_PASSWORD,
                };
                _Context.Customers.Add(result);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task UpdateCustomer(CreateEditCustomerReqModel input, IFormFile avatar)
        {
            var customer = _Context.Customers.Where(c => c.Id == input.Id && !c.IsDeleted).FirstOrDefault();
            if (customer == null)
            {
                throw new NotFoundException(nameof(input.FullName));
            }
            customer.FullName = input.FullName;
            customer.DoB = input.DoB;
            customer.PhoneNumber = input.PhoneNumber;
            customer.Email = input.Email;
            customer.CCCD = input.CCCD;
            customer.Address = input.Address;
            customer.SDTZalo = input.SDTZalo;
            customer.PathAvatar = CommonFunctions.SaveImgLocal(avatar, customer.Id, customer.PathAvatar, "Avatar", FeatureCode.Customer);
            _Context.Customers.Update(customer);
            await _Context.SaveChangesAsync();
        }
        public void DeteleCustomer(long contractId)
        {
            
        }
        public List<ViewBillByEmailResModel> ViewBillByEmail(string email)
        {
            var bill = _Context.Bills.Include(b => b.Customer).Where(b => b.Customer.Email.ToLower() == email.ToLower() && b.Status == CommonEnums.StatusBill.DaXacNhanThanhToan).Select(record => new ViewBillByEmailResModel
            {
                BillID = CommonFunctions.Encryption(record.Id.ToString()),
                Amount = record.TotalAmount,
                PaymentDate = record.PaymentDate.ToString("dd/MM/yyyy"),
                RoomName = record.Room.Name,
                AddressTower = record.Room.Tower.Name,
                Time = record.CreationDate.AddMonths(-1).ToString("MM/yyyy"),
            }).ToList();
            if(bill.Count == 0)
            {
                throw new Exception($"Không có hoá đơn nào với địa chỉ Email bạn vừa nhập");
            }
            return bill;
        }
        public CustomerResModel GetByDetail(long id) 
        {
            var customer = _Context.Customers.GetAvailableById(id);
            if(_Context.ContractCustomers.Any(item => item.CustomerId == id && item.Contract.TerminationDate == null && !item.Contract.IsDeleted))
            {
                throw new Exception($"Khách hàng {customer.FullName} đã có hợp đồng thuê nhá");
            }
            return CustomerResModel.Mapping(customer,false);
        }
        public CustomerResModel GetInfoUser(long id)
        {
            var customer = _Context.Customers.GetAvailableById(id);
            return CustomerResModel.Mapping(customer,false);
        }
        public long Login(string email, string password)
        {
            return _Context.Customers.Where(item=> item.Email != null && item.Email.ToLower() == email.ToLower() && item.Password == password && !item.IsDeleted).Select(record=> record.Id).FirstOrDefault();
        }
        public async Task ChangePassword(ChangePasswordReqModel input)
        {
            var customer = _Context.Customers.GetAvailableById(input.Id);
            if (customer.Password != input.PasswordOld)
            {
                throw new Exception("Mật khẩu không chính xác");
            }
            customer.Password = input.PasswordNew;
            _Context.Customers.Update(customer);
            await _Context.SaveChangesAsync();
        }
        public ContactInfoResModel GetContactInfo(long id)
        {
            var infoContact = _Context.Customers.GetAvailableById(id);
            return new ContactInfoResModel
            {
                PathAvatar = CommonFunctions.ConverPathIMG(infoContact.PathAvatar),
                PhoneNumber = infoContact.PhoneNumber,
                SDTZalo = infoContact.SDTZalo,
                FullName = infoContact.FullName,

            };
        }
        public List<GetSaveRoomByCustomerResModel> GetSaveRoom(long customerId)
        {
            var saveRoom = _Context.SaveRooms.Include(s => s.Room).ThenInclude(r => r.Tower)
                .Where(item => item.CustomerId == customerId)
                .Select(record => new GetSaveRoomByCustomerResModel
                {
                    RoomId = record.RoomId,
                    TowerName = record.Room.Name,
                    TowerAddress = record.Room.Tower.Address,
                    Device = record.Room.Equipment,
                    Price = record.Room.PriceRoom,
                    Img = CommonFunctions.ConverPathListIMG(_Context.ImgRooms.Where(item => item.RoomId == record.RoomId).Select(i => i.Path).ToList())
                }).ToList();
            return saveRoom;
        }
        public List<GetAllPostByFindPeopleResModel> GetSavePost(long customerId)
        {
            var savePost = _Context.SaveNews.Include(item => item.New)
                .Where(item => item.CustomerId == customerId)
                .Select(record => new GetAllPostByFindPeopleResModel
                {
                    Id = record.NewId,
                    Address = record.New.Address,
                    Gender = record.New.Gender == 1 ? "Nam" : "Nữ",
                    Name = record.New.Name,
                    Price = record.New.PriceRoom,
                    Img = CommonFunctions.ConverPathListIMG(_Context.NewRoomPhotos.Where(item => item.NewId == record.NewId).Select(i => i.Path).ToList())
                }).ToList();
            return savePost;
        }
    }
}
