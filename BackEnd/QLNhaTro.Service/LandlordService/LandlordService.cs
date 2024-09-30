using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.LandlordService
{
    public class LandlordService : ILandlordService
    {
        private readonly AppDbContext _Context;
        public LandlordService(AppDbContext context)
        {
            _Context = context;
        }
        public async Task<bool> CreateEditLandlord(CreateEditLandlordReqModels input)
        {
            
            if(input.LandlordId <= 0)
            {
                try
                {
                    var landlord = new Landlord
                    {
                        FullName = input.FullName,
                        DoB = input.DoB,
                        PhoneNumber = input.PhoneNumber,
                        Email = input.Email,
                        CCCD = input.CCCD,
                        Address = input.Address,
                    };
                    _Context.Landlord.Add(landlord);
                    await _Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                    throw;
                }
            }
            else
            {
                var landlord = _Context.Landlord.Where(record => record.Id == input.LandlordId && record.IsDeleted == false).FirstOrDefault();
                if (landlord == null)
                {
                    return false;                }
                try
                {   
                    landlord = new Landlord
                    {
                        FullName = input.FullName,
                        DoB = input.DoB,
                        PhoneNumber = input.PhoneNumber,
                        Email = input.Email,
                        CCCD = input.CCCD,
                        Address = input.Address,
                    };
                    _Context.Landlord.Update(landlord);
                    await _Context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                    throw;
                }
            }
            return true;
        }
        public async Task<bool> DeleteLandlord(long id)
        {
            var landlord = _Context.Landlord.Where(record => record.Id == id && record.IsDeleted == false).FirstOrDefault();
            if (landlord == null) 
            {
                return false;
            }
            try
            {
                landlord.IsDeleted = true;
                _Context.Landlord.Update(landlord);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                throw;
            }
            return true;
        }
    }
}
