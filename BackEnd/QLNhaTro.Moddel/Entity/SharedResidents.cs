using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class SharedResidents : BaseMasterData
    {
        public string Address { get; set; } //Địa chỉ
        public int Gender { get; set; } // Giới tính
        public decimal PriceRoom { get; set; } //Giá phòng
        public string Describe { get; set; } // Mô tả
        public long CustomerId { get; set; }
        public Customers Customer { get; set; }
        public bool IsFound { get; set; } //Trạng thái bài đăng
    }
}
