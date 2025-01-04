using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Commons
{
    public partial class CommonConstants
    {
        public class DefaultValue
        {
            public const string DEFAULT_CONTROLLER_ROUTER = "api/[controller]/[action]";
            public const string DEFAULT_BASE_Directory_IMG = @"D:\Code\BoardingHouseManagement\BoardingHouseManagement\FrontEnd\public";

        }
        public class AppSettingKeys
        {
            public const string DEFAULT_CONNECTION = "DefaultConnection";
        }
        public class JWT
        {
            public static string Permission = "Permission";
        }
        public class ExceptionMessage
        {
            public const string NOT_FOUND = "{0} không tồn tại.";
            public const string ITEM_NOT_FOUND = "Item not found.";
            public const string ALREADY_EXIST = "{0} đã tồn tại.";
            public const string SUCCESS = "{0} success.";
            public const string SHOULD_GREATER_TODAY = "{0} Date is late.";
            public const string INVALID = "{0} invalid.";
            public const string EMAIL_NOT_ACTIVATED = "Email not activated";
        }
    }
}
