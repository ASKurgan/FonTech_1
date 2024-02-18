using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonTech.Domain.Enum
{
    public enum ErrorCodes
    {
        ReportsNotFound = 0,
        ReportNotFound = 1,
        ReportAllreadyExists = 2,

        UserNotFound = 11,
        UserAllreadyExists = 12,
        UserUnauthorizedAccess = 13,
        UserAllreadyExistsThisRole = 14,


        PasswordNotEqualsPasswordConfirm = 21,
        PasswordIsWrong = 22,

        RoleAlreadyExists = 31,
        RoleNotFound = 32,

        InternalServerError = 10,

    }
}
