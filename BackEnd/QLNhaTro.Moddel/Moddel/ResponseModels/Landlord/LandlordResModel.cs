using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class LandlordResModel
    {
        public required string FullName { get; set; }
        public required DateTime DoB { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string CCCD { get; set; }
        public required string Address { get; set; }
        public string SDTZalo { get; set; }
        public string? PathAvatar { get; set; }
        public static LandlordResModel Mapping(Landlord landlord)
        {
            return new LandlordResModel
            {
                FullName = landlord.FullName,
                DoB = landlord.DoB,
                PhoneNumber = landlord.PhoneNumber,
                Email = landlord.Email,
                CCCD = landlord.CCCD,
                Address = landlord.Address,
                SDTZalo = landlord.SDTZalo,
                PathAvatar = CommonFunctions.ConverPathIMG(landlord.PathAvatar),
            };
        }
    }
}
