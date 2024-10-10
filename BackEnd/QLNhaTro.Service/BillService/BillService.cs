using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Moddel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace QLNhaTro.Service.BillService
{
    public class BillService : IBillService
    {
        private const string vnp_ApiUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";  // API URL của VNPay
        private const string vnp_TmnCode = "ZDDD49MI";  // Mã website tại VNPay
        private const string vnp_HashSecret = "5JUA8NUS8YYXU3Q3LVYFASWE8GWXWF23";  // Chuỗi bí mật để tạo checksum
        private readonly AppDbContext _Context;

        public BillService(AppDbContext context)
        {
            _Context = context;
        }
        public string CreatePaymentUrl(decimal amount, string orderId, string orderInfo, string bankCode = "")
        {
            // Chuyển đổi số tiền về đơn vị VNĐ
            long amountVnd = (long)(amount * 100);

            var vnpayData = new SortedList<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", amountVnd.ToString() },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", orderId },
                { "vnp_OrderInfo", orderInfo },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", "https://yourwebsite.com/api/payment/status" }, // URL xử lý kết quả
                { "vnp_IpAddr", GetIpAddress() },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
            };

            if (!string.IsNullOrEmpty(bankCode))
            {
                vnpayData.Add("vnp_BankCode", bankCode);
            }

            // Tạo URL thanh toán với checksum
            string queryString = BuildQueryString(vnpayData);
            string vnp_SecureHash = GenerateChecksum(vnp_HashSecret, queryString);
            queryString += "&vnp_SecureHash=" + vnp_SecureHash;

            return vnp_ApiUrl + "?" + queryString;
        }
        private static string BuildQueryString(SortedList<string, string> data)
        {
            var queryString = new StringBuilder();
            foreach (var item in data)
            {
                queryString.Append(HttpUtility.UrlEncode(item.Key) + "=" + HttpUtility.UrlEncode(item.Value) + "&");
            }
            return queryString.ToString().TrimEnd('&');
        }

        private static string GenerateChecksum(string hashSecret, string input)
        {
            byte[] hashBytes = Encoding.UTF8.GetBytes(hashSecret + input);
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(hashBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        private static string GetIpAddress()
        {
            return "127.0.0.1"; // IP giả định
        }

    }
}
