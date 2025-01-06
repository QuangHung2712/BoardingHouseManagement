using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            [Description("Chưa có hoá đơn")]
            ChuaDienThongTin = 1,
            [Description("Chưa thanh toán")]
            ChuaThanhToan = 2,
            [Description("Chờ xác nhận thanh toán")]
            ChuaXacNhanThanhToan = 3,
            [Description("Đã thanh toán")]
            DaXacNhanThanhToan = 4
        }
        public enum NotificationType
        {
            XacNhanHoaDon = 1,
            ThongBaoTuHeThong = 2,
        }
    }
}
