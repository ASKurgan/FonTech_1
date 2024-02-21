using AutoMapper;
using FonTech.Application.Resources;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Entity;
using FonTech.Domain.Enum;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Interfaces.Validations;
using FonTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonTech.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IBaseRepository<Report> _reportRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IReportValidator _reportValidator;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        public ReportService(IBaseRepository<Report> reportRepository, IBaseRepository<User> userRepository,
            IReportValidator reportValidator, IMapper mapper, Serilog.ILogger logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
            _reportValidator = reportValidator;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        /// <inheritdoc /> </inheritdoc>

        public async Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto dto)
        {
            
                User? user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.UserId);
                Report? report = await _reportRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.Name);
                BaseResult? result = _reportValidator.CreateValidator(report, user);

                if (!result.IsSuccess)
                {
                    return new BaseResult<ReportDto>()
                    {
                        ErrorMessage = result.ErrorMessage,
                        ErrorCode = result.ErrorCode,
                    };
                }

                report = new Report()
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    UserId = user.Id,
                };

                await _reportRepository.CreateAsync(report);
                return new BaseResult<ReportDto>()
                {
                    Data = _mapper.Map<ReportDto>(report)
                };
            
        }

        public async Task<BaseResult<ReportDto>> DeleteReportAsync(long id)
        {
            
                //await _reportRepository.CreateAsync(new Report()
                //{

                //    UserId = 1,
                //    Name = "ASAAA",
                //    Description = "Uytre bhf ddd",
                //    CreatedAt = DateTime.UtcNow,
                //    CreatedBy = 1,
                //    UpdatedAt = DateTime.UtcNow,
                //    UpdatedBy = 1,
                //});

                Report? report = await _reportRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                BaseResult? result = _reportValidator.ValidateOnNull(report);

                if (!result.IsSuccess)
                {
                    return new BaseResult<ReportDto>()
                    {
                        ErrorMessage = result.ErrorMessage,
                        ErrorCode = result.ErrorCode,
                    };
                }

                 _reportRepository.Remove(report);

                await _reportRepository.SaveChangesAsync();

                return new BaseResult<ReportDto>()
                {
                    Data = _mapper.Map<ReportDto>(report)
                };

        }

        public BaseResult<ReportDto> GetReportByIdAsync(long id)
        {
            ReportDto? report;
            try
            {
                report = _reportRepository.GetAll()
                    .AsEnumerable()
                    .Select(x => new ReportDto(x.Id, x.Name, x.Description, x.CreatedAt.ToLongDateString()))
                    .FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }

            if (report == null)
            {
                _logger.Warning($"Отчёт с {id} не найден", id);

                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = ErrorMessage.ReportNotFound,
                    ErrorCode = (int)ErrorCodes.ReportNotFound
                };
            }

            return new BaseResult<ReportDto>()
            {
                Data = report
            };
        }


        /// <inheritdoc /> </inheritdoc>


        public async Task<CollectionResult<ReportDto>> GetReportsAsync(long userId)
        {
            ReportDto[] reports;
            try
            {
                reports = await _reportRepository.GetAll()
                    .Where(x => x.UserId == userId)
                    .Select(x => new ReportDto(x.Id, x.Name, x.Description, x.CreatedAt.ToLongDateString()))
                    .ToArrayAsync();

            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new CollectionResult<ReportDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }

            if (!reports.Any())
            {
                _logger.Warning(ErrorMessage.ReportsNotFound, reports.Length);

                return new CollectionResult<ReportDto>()
                {
                    ErrorMessage = ErrorMessage.ReportsNotFound,
                    ErrorCode = (int)ErrorCodes.ReportsNotFound
                };
            }

            return new CollectionResult<ReportDto>()
            {
                Data = reports,
                Count = reports.Length
            };
        }

        public async Task<BaseResult<ReportDto>> UpdateReportAsync(UpDateReportDto dto)
        {
            try
            {
                Report? report = await _reportRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);

                BaseResult? result = _reportValidator.ValidateOnNull(report);

                if (!result.IsSuccess)
                {
                    return new BaseResult<ReportDto>()
                    {
                        ErrorMessage = result.ErrorMessage,
                        ErrorCode = result.ErrorCode,
                    };
                }

              var  reportNew = new Report()
                {
                   Name = dto.Name,
                   Description = dto.Description,
                };

              //  report.Name = dto.Name;
              //  report.Description = dto.Description;

               var updatedReport = _reportRepository.Update(reportNew);

               await _reportRepository.SaveChangesAsync();   

                return new BaseResult<ReportDto>()
                {
                    Data = _mapper.Map<ReportDto>(updatedReport)
                };
            }

            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}
