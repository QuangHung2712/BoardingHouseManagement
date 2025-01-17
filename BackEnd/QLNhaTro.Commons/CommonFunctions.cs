using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonConstants;
using static QLNhaTro.Commons.CommonEnums;

namespace QLNhaTro.Commons
{
    public static class CommonFunctions
    {
        private static readonly string Key = "kkFVQVMYIdJglUxs";
        private static readonly string IV = "dLnolzxmIyvmcfSD";

        public static T GetAvailableById<T>(this DbSet<T> entity, long id) where T : BaseEntityIsDelete
        {
            var record = entity.Where(c => c.Id == id && !c.IsDeleted).FirstOrDefault();
            if (record == null)
            {
                throw new Exception(CommonConstants.ExceptionMessage.ITEM_NOT_FOUND);
            }
            return record;
        }
        public static void IsGetAvailableById<T>(this DbSet<T> entity, long id) where T : BaseEntityIsDelete
        {
            var record = entity.Where(c => c.Id == id && !c.IsDeleted).FirstOrDefault();
            if (record == null)
            {
                throw new Exception(CommonConstants.ExceptionMessage.ITEM_NOT_FOUND);
            }
        }
        public static void Delete<T>(this DbSet<T> entity, long id, bool isPermanent = false) where T : BaseEntityIsDelete
        {
            var record = entity.GetAvailableById(id);
            if (isPermanent)
            {

            }
            else
            {
                record.IsDeleted = true;
                entity.Update(record);
            }
        }
        public static string Encryption(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }
        public static string Decrypt(string cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
        public static string ConverPathIMG(string input)
        {
            if(input != null)
            {
                string path = Path.GetRelativePath(DefaultValue.DEFAULT_BASE_Directory_IMG, input);
                var result = path.Replace("\\", "/");
                return "/" + result;
            }
            return "";
        }
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
        public static string SaveImgLocal(IFormFile input, long userId, string PathImgQROld, string nameFile,FeatureCode user)
        {
            if (!string.IsNullOrEmpty(PathImgQROld) && PathImgQROld != DefaultValue.DEFAULT_IMG_AVATAR)
            {
                File.Delete(PathImgQROld);
            }
            // Tạo tên ảnh
            string fileName = nameFile + Path.GetExtension(input.FileName);

            // Đường dẫn thư mục lưu ảnh
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $@"{DefaultValue.DEFAULT_BASE_Directory_IMG}\images\UserInformation\{user.ToString()}\{userId}");

            // Kiểm tra và tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Đường dẫn đầy đủ của ảnh
            string filePath = Path.Combine(directoryPath, fileName);

            // Lưu file vào đường dẫn đã chỉ định
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                input.CopyTo(stream);
            }

            // Trả về đường dẫn ảnh đã lưu
            return filePath;
        }
        public static void CoppyFile(string OriginalPath, string NewPath)
        {
            try
            {
                // Copy ảnh từ sourcePath sang destinationPath
                File.Copy(OriginalPath, NewPath, overwrite: true); // `overwrite: true` cho phép ghi đè nếu file tồn tại
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Lỗi khi copy file: {ex.Message}");
            }
        }
    }
}
