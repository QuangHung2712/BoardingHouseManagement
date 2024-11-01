using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using QLNhaTro.Commons;
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
        public async Task<LandlordResModel> GetDetail(long landlordId)
        {
            return await _Service.GetDetail(landlordId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLandlord([FromBody] CreateEditLandlordReqModels data)
        {
            await _Service.UpdateLandlord(data);
            return Ok();
        }

        [HttpDelete("{landlordId}")]
        public IActionResult Delete(long landlordId)
        {
            _Service.DeleteLandlord(landlordId);
            return Ok();
        }
    }
}
