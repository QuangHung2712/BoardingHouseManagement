using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.BillService;
using QLNhaTro.Service.RoomService;

namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillController(IBillService billService)
        {
            _billService = billService;
        }
        [HttpPost]
        public async Task<ActionResult> SendRequestBill()
        {
            try
            {
                await _billService.SubmitRequesInformation();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpGet("{billId}")]
        public IActionResult GetInfoBill(string billId)
        {
            try
            {
                var result = _billService.GetInfoBill(billId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpPut]
        public async Task<ActionResult> CalculateInvoice([FromBody] GetInfoBillResModel input)
        {
            try
            {
                await _billService.CalculateInvoice(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpPut]
        public async Task<ActionResult> PayBill([FromQuery] long biilId)
        {
            try
            {
                await _billService.PayBill(biilId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpGet]
        public IActionResult GetRequestPayment([FromQuery] long landlordId)
        {
            try
            {
                var result =  _billService.GetRequestPayment(landlordId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpPut]
        public async Task<ActionResult> RefusePay([FromBody] RefusePayReqModel data)
        {
            try
            {
                await _billService.RefusePay(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpPut]
        public async Task<ActionResult> AcceptPayments([FromBody] RefusePayReqModel data)
        {
            try
            {
                await _billService.AcceptPayments(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] long towerId)
        {
            try
            {
                var result = _billService.GetAll(towerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
        [HttpDelete("{billId}")]
        public async Task<ActionResult> Delete(long billId)
        {
            try
            {
                await _billService.DeleteBill(billId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpGet]
        public IActionResult GetDetail([FromQuery] long billId)
        {
            try
            {
                var result = _billService.GetDetail(billId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
    }
}
