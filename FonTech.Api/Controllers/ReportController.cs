using Asp.Versioning;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Entity;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Получение отчётов пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <remarks>   
        /// Sample request
        /// 
        ///    GET   
        /// 
        ///       {
        ///       
        ///        "id": 1
        ///      
        ///       }
        ///   
        /// </remarks>
        /// <response code = "200"> Если отчёты были найдены </response>>
        /// <response code = "400"> Если отчёты не были найдены </response>>

        [HttpGet(template: "reports/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDto>>> GetUserReports(long userId)
        {
            CollectionResult<ReportDto>? response = await _reportService.GetReportsAsync(userId);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Получение отчёта пользователя с указанием идентификатора
        /// </summary>
        /// <param name="id"></param>
        /// <remarks> 
        /// Sample request
        /// 
        ///    GET  
        /// 
        ///       {
        ///       
        ///        "id": 1
        ///      
        ///       }
        ///  
        /// </remarks>
        /// <response code = "200"> Если отчёт был найден </response>>
        /// <response code = "400"> Если отчёт не был найден </response>>

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

        /// <summary>
        /// Удаление отчёта пользователя с указанием идентификатора
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>  
        /// Sample request
        /// 
        ///    DELETE 
        /// 
        ///       {
        ///       
        ///        "id": 1
        ///      
        ///       }
        ///       
        /// </remarks>
        /// <response code = "200"> Если отчёт был удалён </response>>
        /// <response code = "400"> Если отчёт не был удалён </response>>

        [HttpDelete(template: "{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDto>>> Delete(long id)
        {
            BaseResult<ReportDto>? response = await _reportService.DeleteReportAsync(id);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Создание отчёта
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        /// Request for create report
        ///   
        ///   POST
        ///      
        ///       {
        ///       
        ///         "name": "Test #1",
        ///       
        ///         "description": "Test#1 - Description",
        ///         
        ///         "userId": 1
        ///      
        ///       }
        ///      
        /// </remarks>
        /// <response code = "200"> Если отчёт создался </response>>
        /// <response code = "400"> Если отчёт не был создан </response>>


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDto>>> Create([FromBody] CreateReportDto dto)
        {
            BaseResult<ReportDto>? response = await _reportService.CreateReportAsync(dto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Обновление отчёта с указанием основных свойств
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        /// Request for update report
        ///   
        ///   PUT
        ///      
        ///       {
        ///       
        ///         "id": 1
        ///       
        ///         "name": "Test #2",
        ///       
        ///         "description": "Test#2 - Description",
        ///         
        ///       }
        ///      
        /// </remarks>
        /// <response code = "200"> Если отчёт обновился </response>>
        /// <response code = "400"> Если отчёт не был обновлён </response>>

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDto>>> Update([FromBody] UpDateReportDto dto)
        {
            var response = await _reportService.UpdateReportAsync(dto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
