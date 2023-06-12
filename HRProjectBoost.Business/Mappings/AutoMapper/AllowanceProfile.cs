using AutoMapper;
using HRProjectBoost.DTOs.DTOs.Allowance;
using HRProjectBoost.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Business.Mappings.AutoMapper
{
    public class AllowanceProfile :Profile
    {
        public AllowanceProfile()
        {
            CreateMap<Allowance,AllowanceDto>().ReverseMap();
            CreateMap<Allowance,AllowanceCreateDto>().ReverseMap();
        }
    }
}
