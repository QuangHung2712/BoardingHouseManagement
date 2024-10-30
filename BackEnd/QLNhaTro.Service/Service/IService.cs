using Microsoft.AspNetCore.Http;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaTro.Service.Service
{
    public interface IService
    {
        Task DeleteService(long Id);
        Task CreateEditService(CreateEditServiceReqModel input);
        Task<List<GetAllServiceResModel>> GetAllEntity(long towerId);
        byte[] ExportServiceToExcel(long towerId);
        Task ImportServiceToExcel(IFormFile fileInput);
    }
}
