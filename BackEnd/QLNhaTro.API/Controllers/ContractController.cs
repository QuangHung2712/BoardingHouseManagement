using Microsoft.AspNetCore.Mvc;
using QLNhaTro.Commons;
using QLNhaTro.Moddel.Moddel.RequestModels;
using QLNhaTro.Moddel.Moddel.ResponseModels;
using QLNhaTro.Service.ContractService;

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
            return await _Contract.GetAllContractByTowerId(towerId);
        }

        [HttpGet]
        public async Task<GetDetailContractResModel> GetDetail([FromQuery] long contractId)
        {
            return await _Contract.GetDetail(contractId);
        }

        [HttpPost]        
        public async Task<IActionResult> CreateEdit([FromBody] CreateEditContractReqModel data)
        {
            await _Contract.CreateEditContract(data);
            return Ok();
        }

        [HttpDelete("{contractId}")]
        public IActionResult Delete(long contractId) 
        {
            _Contract.DeleteContract(contractId);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Extend([FromBody] ContractExtensionReqModel data)
        {
            await _Contract.ContractExtension(data);
            return Ok();
        }

        [HttpPost("{contractId}")]
        public IActionResult ExportWord(long contractId)
        {
            string outputPath = _Contract.ExportWord(contractId);
            return PhysicalFile(outputPath, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "contract.docx");

        }
    }
}
