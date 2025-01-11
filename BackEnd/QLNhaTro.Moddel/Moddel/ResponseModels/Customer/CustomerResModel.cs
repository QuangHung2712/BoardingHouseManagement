using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class CustomerResModel : BaseEntity
    {
        public string FullName { get; set; }
        public DateTime DoB { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CCCD { get; set; }
        public string Address { get; set; }
        public string? SDTZalo { get; set; }
        public bool IsRepresentative { get; set; }//Có phải là người đại diện không
        public bool IsSystemRegistrant { get; set; }//Có phải người đăng ký hệ thống không
        public string? PathAvatar { get; set; }

        public static CustomerResModel Mapping(Customers customer,bool isRepresentative)
        {
            return new CustomerResModel
            {
                Id = customer.Id,
                FullName = customer.FullName,
                DoB = customer.DoB,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                CCCD = customer.CCCD,
                Address = customer.Address,
                IsRepresentative = isRepresentative,
                IsSystemRegistrant = customer.Password != null,
                PathAvatar = customer.PathAvatar,
                SDTZalo = customer.SDTZalo,
            };
        }
    }
}
