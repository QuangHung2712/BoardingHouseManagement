using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonConstants;

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
            string path = Path.GetRelativePath(DefaultValue.DEFAULT_BASE_Directory_IMG, input);
            var result = path.Replace("\\", "/");
            return "/" + result;
        }
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}
