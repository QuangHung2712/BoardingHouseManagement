using Microsoft.EntityFrameworkCore;
using QLNhaTro.Commons;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.IncurService
{
    public class IncurService : IIncurService
    {
        private readonly AppDbContext _Context;

        public IncurService(AppDbContext context)
        {
            _Context = context;
        }
        public Task<List<GetAllIncurResModel>> GetAllByTower(long towerId)
        {
            var incurData = _Context.Incurs.Where(i => !i.IsDeleted && i.TowerId == towerId).Select(i => new GetAllIncurResModel
            {
                Id = i.Id,
                RoomId = i.RoomId,
                RoomName = i.Room.Name,
                CreationDate = i.CreationDate,
                Amount = i.Amount,
                Reason = i.Reason,
                StatusPay = i.StatusPay
            }).OrderByDescending(x=> x.CreationDate).ToListAsync();
            return incurData;
        }
        public async Task CreateEditIncur(CreateEditIncurReqModel input)
        {
            if (input.Id <= 0)
            {
                try
                {
                    var newIncur = new Incur
                    {
                        RoomId = input.RoomId,
                        Amount = input.Amount,
                        CreationDate = DateTime.Now,
                        Reason = input.Reason,
                        TowerId = input.TowerId,
                    };
                    _Context.Incurs.Add(newIncur);
                    await _Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
            else
            {
                try
                {
                    var incur = _Context.Incurs.GetAvailableById(input.Id);
                    if (incur.StatusPay == true) throw new Exception("Phát sinh đã được thanh toán không thể sửa");
                    incur.RoomId = input.RoomId;
                    incur.Amount = input.Amount;
                    incur.Reason = input.Reason;
                    _Context.Incurs.Update(incur);
                    await _Context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
        }
        public async Task DeleteIncur(long Id)
        {
            var incur = _Context.Incurs.GetAvailableById(Id);
            if (incur.StatusPay == true) throw new Exception("Phát sinh đã được thanh toán không thể xoá");
            incur.IsDeleted = true;
            await _Context.SaveChangesAsync();
        }
    }
}
