using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
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
        public async Task<LandlordResModel> GetDetail(long id)
        {
            try
            {
                Landlord landlord = await GetById(id);
                return LandlordResModel.Mapping(landlord);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task CreateEditLandlord(CreateEditLandlordReqModels input)
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
                    throw;
                }
            }
            else
            {
                var landlord = _Context.Landlord.Where(record => record.Id == input.LandlordId && record.IsDeleted == false).FirstOrDefault();
                if (landlord == null) throw new NotFoundException(nameof(input.FullName));
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
                    throw;
                }
            }
        }
        public async Task DeleteLandlord(long id)
        {
            var landlord = _Context.Landlord.Where(record => record.Id == id && record.IsDeleted == false).FirstOrDefault();
            if (landlord == null) throw new NotFoundException(nameof(id));
            try
            {
                landlord.IsDeleted = true;
                _Context.Landlord.Update(landlord);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task<Landlord> GetById(long id)
        {
            try
            {
                IQueryable<Landlord> landlordQuery = _Context.Landlord.Where(e => e.Id == id).AsQueryable();
                if (landlordQuery.IsNullOrEmpty()) throw new NotFoundException(nameof(Landlord.Id));
                return await landlordQuery.FirstAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
