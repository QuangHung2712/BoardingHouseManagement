using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Service.BillService;

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
        public IActionResult CreatePayment([FromBody] BillPaymentRequest request)
        {
            // Xử lý tạo URL thanh toán
            string paymentUrl = _billService.CreatePaymentUrl(request.Amount, request.OrderId, request.OrderInfo, request.BankCode);
            return Ok(new { url = paymentUrl });
        }
    }
}
