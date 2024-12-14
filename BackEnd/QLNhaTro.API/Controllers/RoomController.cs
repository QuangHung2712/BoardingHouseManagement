using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.ContractService;
using QLNhaTro.Service.RoomService;


namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService roomService;
        private readonly IContractService contractService;

        public RoomController(IRoomService roomService, IContractService contractService)
        {
            this.roomService = roomService;
            this.contractService = contractService;
        }

        [HttpGet("{towerId}")]
        public async Task<ActionResult<List<GetAllRoomResModel>>> GetAllRoom(long towerId)
        {
            return await roomService.GellAllRoomByTower(towerId);
        }

        [HttpGet]
        public async Task<GetRoomDetailByIdResModel> GetDetail([FromQuery] long roomId)
        {
            return await roomService.GetDetailRoomById(roomId);
        }

        [HttpGet]
        public async Task<GetContractByRoomIDResModel> GetContractByRoomId([FromQuery] long roomId)
        {
            return await contractService.GetContractByRoomId(roomId);
        }

        [HttpGet]
        public async Task<ActionResult<List<GetDropDownRoom>>> GetDropDown([FromQuery] long towerId)
        {
            return await roomService.GetDropDownRooms(towerId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEditRoom([FromBody] CreateEditRoomReqModel data)
        {
            await roomService.CreateEditRoom(data);
            return Ok();
        }

        [HttpPut("{roomId}")]
        public async Task<IActionResult> FineNewCustomers(long roomId)
        {
            await roomService.FineNewCustomers(roomId);
            return Ok();
        }

        [HttpDelete("{RoomId}")]
        public IActionResult DeleteRoom(long RoomId) 
        {
            roomService.DeleteRoom(RoomId);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> CheckOutRoom([FromBody] CheckOutRoomReqModel data)
        {
            await roomService.CheckOut(data);
            //Thực hiện việc quyết toán tiền cho khách
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeRoom([FromBody] ChangeRoomReqModel data)
        {
            await roomService.ChangeRoom(data);
            //Xử lý việc chênh tiền trọ
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<GetRoomNoContract>>> GetRoomNoContract([FromQuery] long towerID)
        {
            return await roomService.GetRoomNoContract(towerID);
        }
    }
}
