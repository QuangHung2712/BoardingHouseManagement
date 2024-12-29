using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Commons
{
    public partial class CommonEnums
    {
        public enum FeatureCode
        {
            Landlord = 0,
            Customer = 1,
        }
        public enum StatusBill
        {
            ChuaDienThongTin = 1,
            ChuaThanhToan = 2,
            ChuaXacNhanThanhToan = 3,
            DaXacNhanThanhToan = 4
        }
    }
}
