using FonTech.Domain.Dto.Report;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers
{
    //  [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BaseResult<ReportDto>> GetReport(long id)
        {
            BaseResult<ReportDto> response = _reportService.GetReportByIdAsync(id);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);


        }
    }
}
