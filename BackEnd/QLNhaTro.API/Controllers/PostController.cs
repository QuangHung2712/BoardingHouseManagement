using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.Post;
using QLNhaTro.Service.RoomService;


namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet("{postId}")]
        public IActionResult GetDetail(long postId)
        {

            try
            {
                var result = _postService.GetDetail(postId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
        [HttpGet]
        public IActionResult GetByCustomer([FromQuery]long customerId)
        {

            try
            {
                var result = _postService.GetByCustomer(customerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
        [HttpPut]
        public async Task<IActionResult> CreateEdit([FromForm] CreateEditPostReqModel data, [FromForm] List<IFormFile> ImgRoom)
        {
            try
            {
                await _postService.CreateEdit(data, ImgRoom);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
        [HttpDelete("{postId}")]
        public async Task<ActionResult> Delete(long postId)
        {
            try
            {
                await _postService.DetelePost(postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
        [HttpPut]
        public async Task<ActionResult> Found([FromQuery] long postId)
        {
            try
            {
                await _postService.Found(postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
        [HttpPut]
        public async Task<ActionResult> Repost([FromQuery] long postId)
        {
            try
            {
                await _postService.Repost(postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }

        [HttpGet]
        public IActionResult SearchPost([FromQuery] string address, [FromQuery] decimal priceFrom, [FromQuery] decimal priceArrive, [FromQuery] long customerId, [FromQuery] int gender)
        {
            try
            {
                var result = _postService.FindPeople(address, priceFrom, priceArrive, customerId, gender);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SavePost([FromQuery] long customerId, [FromQuery] long postId, [FromQuery] bool status)
        {
            try
            {
                await _postService.SavePost(customerId, postId, status);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
        [HttpGet]
        public IActionResult GetDetailFindPeople([FromQuery] long postId, [FromQuery] long customerId)
        {
            try
            {
                var result = _postService.GetDetailPostByFind(postId, customerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });

            }
        }
    }
}
