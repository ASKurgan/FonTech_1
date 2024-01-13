using FonTech.Domain.Entity;
using FonTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonTech.Domain.Interfaces.Validations
{
    public interface IReportValidator : IBaseValidator<Report>
    {

        /// <summary>
        /// Проверяется наличие отчёта. Если отчёт с переданным названием уже есть в БД,
        /// то создать точно такой же нельзя.
        /// Проверяется пользователь. Если с UserId пользователь не найден, то такого пользователя нет
        /// <param name="report"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        BaseResult CreateValidator(Report report, User user);
    }
}
