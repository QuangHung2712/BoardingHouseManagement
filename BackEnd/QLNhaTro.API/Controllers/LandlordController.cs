using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using QLNhaTro.Commons;
using QLNhaTro.Commons.CustomException;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service;
using QLNhaTro.Service.EmailService;
using QLNhaTro.Service.LandlordService;

namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class LandlordController : ControllerBase
    {
        private readonly ILandlordService _Service;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public LandlordController(ILandlordService service, IAuthService authService, IEmailService emailService)
        {
            _Service = service;
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginReqModels request)
        {
            try
            {
                long landLordId = _Service.Login(request);
                if (landLordId == 0)
                {
                    return Unauthorized(new { Message = "Tài khoản hoặc mật khẩu không đúng!" });
                }
                var result = new LoginResModel
                {
                    Token = _authService.GenerateTokeLandlord(),
                    UserId = landLordId,
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }

        }

        [HttpGet("{landlordId}")]
        public IActionResult GetDetail(long landlordId)
        {
            try
            {
                var result = _Service.GetDetail(landlordId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEditLandlordReqModels data)
        {

            try
            {
                await _Service.CreateLandlord(data);
                await _emailService.SendEmailCreate(data.Email, "defaultpassword");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLandlord([FromForm] CreateEditLandlordReqModels data, [FromForm] IFormFile ImgAvatar)
        {

            try
            {
                await _Service.UpdateLandlord(data, ImgAvatar);
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
        [HttpPost("{email}")]
        public async Task<IActionResult> ForGotPassword(string email)
        {
            try
            {
                bool isEmail = _Service.ForgotPassword(email);
                if (!isEmail)
                {
                    return Unauthorized(new { Message = "Địa chỉ Email không tồn tại!" });
                }
                var result = await _emailService.SendEmailCode(email, "quên mật khẩu");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpPut]
        public async Task<IActionResult> ChangePassword(ChangePasswordReqModel input)
        {
            try
            {
                await _Service.ChangePassword(input);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }

        }
        [HttpGet]
        public ActionResult GetInfoPayment([FromQuery] long id)
        {
            try
            {
                return Ok(_Service.GetInfoPayment(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateInfoPayment([FromForm] UpdateInfoPaymentReqModel data, [FromForm] IFormFile paymentQRImageLink)
        {
            try
            {
                await _Service.UpdateInfoPayment(data, paymentQRImageLink);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }

        }
        [HttpGet]
        public ActionResult GetInfoContact([FromQuery] long id)
        {
            try
            {
                var result = _Service.GetContactInfo(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
