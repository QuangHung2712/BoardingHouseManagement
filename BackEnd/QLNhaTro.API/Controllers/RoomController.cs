using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
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
            try
            {
                var result = await roomService.GellAllRoomByTower(towerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpGet]
        public async Task<ActionResult<GetRoomDetailByIdResModel>> GetDetail([FromQuery] long roomId)
        {

            try
            {
                var result = await roomService.GetDetailRoomById(roomId);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpGet]
        public async Task<ActionResult<GetContractByRoomIDResModel>> GetContractByRoomId([FromQuery] long roomId)
        {
            try
            {
                var result = await contractService.GetContractByRoomId(roomId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpGet]
        public async Task<ActionResult<List<GetDropDownRoom>>> GetDropDown([FromQuery] long towerId)
        {
            try
            {
                var result = await roomService.GetDropDownRooms(towerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEditRoom([FromForm] CreateEditRoomReqModel data,[FromForm] List<IFormFile> ImgRoom)
        {
            try
            {
                await roomService.CreateEditRoom(data, ImgRoom);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpPut("{roomId}")]
        public async Task<IActionResult> FineNewCustomers(long roomId)
        {
            try
            {
                await roomService.FineNewCustomers(roomId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
        [HttpPut]
        public async Task<IActionResult> CancelFineNewCustomers([FromQuery]long roomId)
        {
            try
            {
                await roomService.CancelFineNewCustomers(roomId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpDelete("{RoomId}")]
        public IActionResult DeleteRoom(long RoomId) 
        {
            try
            {
                roomService.DeleteRoom(RoomId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
        [HttpGet]
        public IActionResult GetInfoCheckOut([FromQuery] long roomId)
        {
            try
            {
                var result = roomService.GetInfoCheckout(roomId);
                //Thực hiện việc quyết toán tiền cho khách
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpPut]
        public async Task<IActionResult> CheckOutRoom([FromBody] CheckOutRoomReqModel data)
        {
            try
            {
                var result =  await roomService.CheckOut(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpPut]
        public async Task<IActionResult> ChangeRoom([FromBody] ChangeRoomReqModel data)
        {
            try
            {
                await roomService.ChangeRoom(data);
                //Xử lý việc chênh tiền trọ
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpGet]
        public async Task<ActionResult<List<GetDropDownRoom>>> GetRoomNoContract([FromQuery] long towerID)
        {
            
            try
            {
                var result = await roomService.GetRoomNoContract(towerID);
                //Xử lý việc chênh tiền trọ
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpGet]
        public IActionResult GetInfoHome([FromQuery] long towerID)
        {
            try
            {
                var result = roomService.GetInfoHome(towerID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult SearchRoom([FromQuery] string address, [FromQuery] decimal priceFrom, [FromQuery] decimal priceArrive)
        {
            try
            {
                var result = roomService.SearchRoom(address, priceFrom,priceArrive);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetDetailFindRoom([FromQuery] long roomId)
        {
            try
            {
                var result = roomService.GetRoomDetailFindRoom(roomId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
    }
}
