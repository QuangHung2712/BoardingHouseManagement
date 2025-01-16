using QLNhaTro.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Moddel.Entity
{
    public class Evaluate : BaseEntity
    {
        public long ReviewerId { get; set; } //Người đánh giá
        public Customers Reviewer { get; set; }
        public long? ReviewRecipientId { get; set; } //Khách hàng nhận đánh giá
        public Customers? ReviewRecipient { get; set; }
        public long? LandlordId { get; set; } //Chủ nhà nhận đánh giá
        public Landlord? Landlord { get; set; } 
        public DateTime EvaluationDate { get; set; } // Ngày đánh giá
        public string EvaluationContent { get; set; }//Nội dung đánh giá
        public long? ReviewFatherId { get; set; } // Trả lời cho đánh giá nào
        public Evaluate? ReviewFather { get; set; }
    }
}
