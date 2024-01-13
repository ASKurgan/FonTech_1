using AutoMapper;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FonTech.Application.Mapping
{
    public class ReportMapping : Profile
    {
        public ReportMapping()
        {
            CreateMap<Report, ReportDto>().ReverseMap();
        }
    }
}
