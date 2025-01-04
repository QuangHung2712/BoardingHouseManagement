using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Entity;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.ContractService;
using System.Diagnostics.Contracts;

namespace QLNhaTro.API.Controllers
{
    [Route(CommonConstants.DefaultValue.DEFAULT_CONTROLLER_ROUTER)]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _Contract;

        public ContractController(IContractService contract)
        {
            _Contract = contract;
        }

        [HttpGet("{towerId}")] 
        public async Task<ActionResult<List<GetAllContractByTowerId>>> GetAll(long towerId)
        {
            try
            {
                return await _Contract.GetAllContractByTowerId(towerId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<GetDetailContractResModel>> GetDetail([FromQuery] long contractId)
        {
            try
            {
                var result = await _Contract.GetDetail(contractId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]        
        public async Task<IActionResult> CreateEdit([FromBody] CreateEditContractReqModel data)
        {
            try
            {
                await _Contract.CreateEditContract(data);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("{contractId}")]
        public async Task<IActionResult> Delete(long contractId) 
        {
            try
            {
                await _Contract.DeleteContract(contractId);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Extend([FromBody] ContractExtensionReqModel data)
        {
            try
            {
                await _Contract.ContractExtension(data);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("{contractId}")]
        public IActionResult ExportWord(long contractId)
        {
            try
            {
                string outputPath = _Contract.ExportWord(contractId);
                return PhysicalFile(outputPath, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "contract.docx");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
