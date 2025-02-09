﻿using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service;
using QLNhaTro.Service.CustomerService;
using QLNhaTro.Service.EmailService;
using QLNhaTro.Service.Service;


namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public CustomerController(ICustomerService customerService, IAuthService authService, IEmailService emailService)
        {
            _customerService = customerService;
            _authService = authService;
            _emailService = emailService;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEditCustomerReqModel data)
        {

            try
            {
                await _customerService.CreateCustomer(data);
                await _emailService.SendEmailCreate(data.Email,CommonConstants.DefaultValue.DEFAULT_PASSWORD,CommonEnums.FeatureCode.Customer);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] CreateEditCustomerReqModel data, [FromForm] IFormFile ImgAvatar)
        {

            try
            {
                await _customerService.UpdateCustomer(data, ImgAvatar);
                return Ok();
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
                await _customerService.ChangePassword(input);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetInfo([FromQuery]long Id)
        {
            try
            {
                var result = _customerService.GetInfoUser(Id);
                return Ok(result);
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
                var result = _customerService.GetContactInfo(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult GetSaveRoom([FromQuery] long id)
        {
            try
            {
                var result = _customerService.GetSaveRoom(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult GetSavePost([FromQuery] long id)
        {
            try
            {
                var result = _customerService.GetSavePost(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
