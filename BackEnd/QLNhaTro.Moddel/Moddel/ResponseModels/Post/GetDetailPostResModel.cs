using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.ResponseModels
{
    public class GetDetailPostResModel : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; } //Địa chỉ
        public int Gender { get; set; } // Giới tính
        public decimal PriceRoom { get; set; } //Giá phòng
        public string Describe { get; set; } // Mô tả
        public List<string> PathImgRoom { get; set; }
    }
}
