using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.Service;
using System.Net;
using System.Net.Http.Headers;

namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IService _service;

        public ServiceController(IService service)
        {
            _service = service; 
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAllServiceResModel>>> GetAll([FromQuery] long towerId)
        {
            var result = await _service.GetAllEntity(towerId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEditService([FromBody] CreateEditServiceReqModel data)
        {
            await _service.CreateEditService(data);
            return Ok();
        }

        [HttpDelete("{serviceId}")]
        public async Task<IActionResult> DeleteService(long serviceId)
        {
            await _service.DeleteService(serviceId);
            return Ok();
        }

        [HttpGet("{towerId}")]
        public IActionResult ExportServiceToExcel(long towerId)
        {
            // Gọi service để tạo file Excel dưới dạng byte array
            var fileContent = _service.ExportServiceToExcel(towerId);

            // Tạo HttpResponse để trả về file Excel
            var result = new FileContentResult(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "DichVu.xlsx"
            };

            return result;
        }

        [HttpPut]
        public IActionResult ImportServiceByExcel([FromForm]IFormFile fileInput)
        {
            if (fileInput == null || fileInput.Length == 0)
            {
                return BadRequest("File không tồn tại.");
            }
            _service.ImportServiceToExcel(fileInput);
            return Ok("Nhập dữ liệu từ Excel thành công");
        }
    }
}
