using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLNhaTro.Commons;
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
        public async Task CreateLandlord(CreateEditLandlordReqModels input)
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
                _Context.Landlords.Add(landlord);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task UpdateLandlord(CreateEditLandlordReqModels input)
        {
            var landlord = _Context.Landlords.GetAvailableById(input.LandlordId);
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
                _Context.Landlords.Update(landlord);
                await _Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task DeleteLandlord(long id)
        {
            var landlord = _Context.Landlords.Where(record => record.Id == id && record.IsDeleted == false).FirstOrDefault();
            if (landlord == null) throw new NotFoundException(nameof(id));
            try
            {
                landlord.IsDeleted = true;
                _Context.Landlords.Update(landlord);
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
                IQueryable<Landlord> landlordQuery = _Context.Landlords.Where(e => e.Id == id).AsQueryable();
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
