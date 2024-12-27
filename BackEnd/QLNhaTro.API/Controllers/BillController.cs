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
        public async Task<ActionResult> SendRequestBill()
        {
            await _billService.SubmitRequesInformation();
            return Ok();
        }
    }
}
