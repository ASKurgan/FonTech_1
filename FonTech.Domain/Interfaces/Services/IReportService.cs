using FonTech.Domain.Dto.Report;
using FonTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис, отвечающий за работу с доменной частью отчёта (Report)
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Получение всех отчётов пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>  </returns>
        Task<CollectionResult<ReportDto>> GetReportsAsync(long userId);

        /// <summary>
        /// Получение отчёта по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResult<ReportDto> GetReportByIdAsync(long id);

        /// <summary>
        /// Создание отчёта с базовыми параметрами
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto dto);

        /// <summary>
        /// Удаление отчёта по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDto>> DeleteReportAsync(long id);

        /// <summary>
        /// Обновление отчёта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDto>> UpdateReportAsync(UpDateReportDto dto);
    }
}
