using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseContext = OfficeOpenXml.LicenseContext;


namespace QLNhaTro.Service.Service
{
    public class ServiceRepo : IService
    {
        private readonly AppDbContext _Context;

        public ServiceRepo(AppDbContext context)
        {
            _Context = context;
        }
        public Task<List<GetAllServiceResModel>> GetAllEntity(long towerId)
        {
            var ServiceData =  _Context.Services.Where(item => item.IsActive && item.TowerId == towerId && !item.IsDeleted).Select(s => new GetAllServiceResModel
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.UnitPrice,
                UnitOfCalculation = s.UnitOfCalculation,
                IsOldNewNumber = s.IsOldNewNumber,
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
                        TowerId = input.TowerId,
                        UnitOfCalculation = input.UnitOfCalculation,
                        IsOldNewNumber = input.IsOldNewNumber,
                        IsActive = true,
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
                    service.UnitOfCalculation = input.UnitOfCalculation;
                    service.IsOldNewNumber = input.IsOldNewNumber;
                    _Context.Services.Update(service);
                    if (input.ApplyPriceServiceAllRoom)
                    {
                        ApplyPriceServiceAllRoom(input.Id, input.TowerId, input.Price);
                    }
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
        public byte[] ExportServiceToExcel(long towerId)
        {
            var serviceData = _Context.Services.Where(record => record.TowerId == towerId).ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Dịch vụ");
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "UnitPrice";

                for (int i = 0; i < serviceData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = serviceData[i].Id;
                    worksheet.Cells[i + 2, 2].Value = serviceData[i].Name;
                    worksheet.Cells[i + 2, 3].Value = serviceData[i].UnitPrice;
                }
                return package.GetAsByteArray();
            }
        }
        public async Task ImportServiceToExcel(IFormFile fileInput)
        {
            var serviceInput = new List<Services>();
            using (var package = new ExcelPackage((Stream)fileInput))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Giả sử dữ liệu nằm ở worksheet đầu tiên
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Bắt đầu từ dòng 2, bỏ qua tiêu đề
                {
                    var service = new Services
                    {
                        Name = worksheet.Cells[row, 2].Text,
                        UnitPrice = decimal.Parse(worksheet.Cells[row, 3].Text)
                    };
                    serviceInput.Add(service);
                }
            }
            _Context.Services.AddRange(serviceInput);
            await _Context.SaveChangesAsync();
        }
    }
}
