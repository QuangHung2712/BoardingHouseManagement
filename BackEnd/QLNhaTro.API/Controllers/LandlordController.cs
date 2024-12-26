using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.LandlordService;

namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class LandlordController : ControllerBase
    {
        private readonly ILandlordService _Service;

        public LandlordController(ILandlordService service)
        {
            _Service = service;
        }

        [HttpGet("{landlordId}")]
        public async Task<ActionResult<LandlordResModel>> GetDetail(long landlordId)
        {
            try
            {
                return await _Service.GetDetail(landlordId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLandlord([FromBody] CreateEditLandlordReqModels data)
        {
            
            try
            {
                await _Service.UpdateLandlord(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("{landlordId}")]
        public IActionResult Delete(long landlordId)
        {
            try
            {
                _Service.DeleteLandlord(landlordId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
