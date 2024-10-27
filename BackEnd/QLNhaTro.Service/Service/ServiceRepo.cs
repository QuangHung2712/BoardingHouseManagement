﻿using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.EntityFrameworkCore;
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

namespace QLNhaTro.Service.Service
{
    public class ServiceRepo : IService
    {
        private readonly AppDbContext _Context;

        public ServiceRepo(AppDbContext context)
        {
            _Context = context;
        }
        public Task<List<GetAllServiceResModel>> GetAllEntity()
        {
            var ServiceData = _Context.Services.Where(item => !item.IsActive).Select(s => new GetAllServiceResModel
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.UnitPrice,
            }).ToListAsync();
            return ServiceData;
        }
        public async Task CreateEditService(CreateEditServiceReqModel input)
        {
            if (input.Id <= 0)
            {
                _Context.Towers.IsGetAvailableById(input.TowerId);
                try
                {
                    var newService = new Services
                    {
                        Id = input.Id,
                        Name = input.Name,
                        UnitPrice = input.Price,
                    };
                    _Context.Services.Add(newService);
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
                    var service = _Context.Services.GetAvailableById(input.Id);
                    service.Name = input.Name;
                    service.UnitPrice = input.Price;
                    if (input.ApplyPriceServiceAllRoom)
                    {
                        ApplyPriceServiceAllRoom(input.Id, input.TowerId, input.Price);
                    }
                    _Context.Services.Update(service);
                    await _Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
        }
        private async void ApplyPriceServiceAllRoom(long Id, long TowerId, decimal Price)
        {
            await _Context.ServiceRooms
                    .Where(s => s.ServiceId == Id && s.Service.TowerId == TowerId)
                    .ExecuteUpdateAsync(s => s.SetProperty(sr => sr.Price, sr => Price));
        }
        public async Task DeleteService(long Id)
        {
            _Context.Services.Delete(Id);
            await _Context.SaveChangesAsync();
        }
    }
}