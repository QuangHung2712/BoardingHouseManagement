using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Commons
{
    public static class CommonFunctions
    {
        public static T GetAvailableById<T>(this DbSet<T> entity, long id) where T : BaseEntityIsDelete
        {
            var record = entity.Where(c=>c.Id == id && !c.IsDeleted).FirstOrDefault();
            if(record == null)
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
    }
}
