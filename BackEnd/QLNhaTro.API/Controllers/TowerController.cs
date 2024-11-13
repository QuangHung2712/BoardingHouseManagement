using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.TowerService;


namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class TowerController : ControllerBase
    {
        private readonly ITowerService _towerService;

        public TowerController(ITowerService towerService)
        {
            _towerService = towerService;
        }
        [HttpGet("{landlordId}")]
        public async Task<ActionResult<List<GetAllTowerResModel>>> GetAllTowerByLandLordId(int landlordId) 
        {
            return await _towerService.GetAllTowerByLandlordId(landlordId);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEditTower([FromBody] CreateEditTowerReqModel data)
        {
            await _towerService.CreateEditTower(data);
            return Ok();
        }
        [HttpPut("towerId")]
        public IActionResult DeleteTower(long towerId)
        {
             _towerService.DeleteTower(towerId);
            return Ok();
        }
    }
}
