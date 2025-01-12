using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Moddel.RequestModels
{
    public class CreateEditPostReqModel : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; } //Địa chỉ
        public int Gender { get; set; } // Giới tính
        public decimal PriceRoom { get; set; } //Giá phòng
        public string Describe { get; set; } // Mô tả
        public long CustomerId { get; set; }
    }
}
