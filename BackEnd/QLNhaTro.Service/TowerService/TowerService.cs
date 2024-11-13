﻿using Microsoft.EntityFrameworkCore;
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

namespace QLNhaTro.Service.TowerService
{
    public class TowerService : ITowerService
    {
        private readonly AppDbContext _Context;

        public TowerService(AppDbContext context)
        {
            _Context = context;
        }
        public async Task<List<GetAllTowerResModel>> GetAllTowerByLandlordId(long LandlordId)
        {
            var towerData = await _Context.Towers.Where(item => item.LandlordId == LandlordId).Select(t => new GetAllTowerResModel
            {
                Id = t.Id,
                Address = t.Address,
                SumRoom = _Context.Rooms.Count(r=> r.TowerId == t.Id),
                RoomRented = 10,
                RoomStillEmpty = 10
            }).ToListAsync();
            if(towerData.Count == 0) throw new NotFoundException(nameof(LandlordId));
            return towerData;
        }
        public async Task CreateEditTower(CreateEditTowerReqModel input)
        {
            if(input.Id <= 0)
            {
                try
                {
                    var newTower = new Tower()
                    {
                        Name = input.TowerName,
                        Address = input.Address,
                        LandlordId = input.LandlordId,
                    };
                    _Context.Towers.Add(newTower);
                    await _Context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                try
                {
                    var tower = _Context.Towers.GetAvailableById(input.Id);
                    tower.Name = input.TowerName;
                    tower.Address = input.Address;
                    _Context.Towers.Update(tower);
                    await _Context.SaveChangesAsync();
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        public async Task DeleteTower(long Id)
        {
            var tower = _Context.Towers.GetAvailableById(Id);
            tower.IsDeleted = true;
            _Context.Towers.Update(tower);
            await _Context.SaveChangesAsync();
        }
    }
}