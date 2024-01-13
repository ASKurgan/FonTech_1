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

        InternalServerError = 10,

    }
}
