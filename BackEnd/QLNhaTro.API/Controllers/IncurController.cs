using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.IncurService;

namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class IncurController : ControllerBase
    {
        private readonly IIncurService _incurService;

        public IncurController(IIncurService incurService)
        {
            _incurService = incurService;
        }

        [HttpGet("{towerId}")]
        public async Task<ActionResult<List<GetAllIncurResModel>>> GetAllIncur(long towerId)
        {
            var result = await _incurService.GetAllByTower(towerId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEditIncur([FromBody] CreateEditIncurReqModel data)
        {
            await _incurService.CreateEditIncur(data);
            return Ok();
        }

        [HttpPut("{incurId}")]
        public IActionResult DeleteIncur(long incurId) 
        {
            _incurService.DeleteIncur(incurId);    
            return Ok();
        }
    }
}
