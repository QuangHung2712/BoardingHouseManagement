using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service;
using QLNhaTro.Service.CustomerService;


namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthService _authService;

        public CustomerController(ICustomerService customerService, IAuthService authService)
        {
            _customerService = customerService;
            _authService = authService;
        }
        [HttpGet]
        public IActionResult GetBillByEmail([FromQuery] string email) 
        {
            try
            {
                var result = _customerService.ViewBillByEmail(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpGet("{Id}")]
        public IActionResult GetDetail(long Id) 
        {
            try
            {
                var result = _customerService.GetByDetail(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginReqModels request)
        {
            try
            {
                var customer = _customerService.Login(request.Email, request.Password);
                if (customer == 0)
                {
                    return Unauthorized(new { Message = "Tài khoản hoặc mật khẩu không đúng!" });
                }
                var result = new LoginResModel
                {
                    Token = _authService.GenerateTokeCustomer(),
                    UserId = customer,
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
