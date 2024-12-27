using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        public static string Decrypt(string plainText)
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
    }
}
