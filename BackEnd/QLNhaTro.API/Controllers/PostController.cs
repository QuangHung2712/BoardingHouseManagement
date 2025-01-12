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
        public async Task<ActionResult> DeleteRoom(long postId)
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
    }
}
