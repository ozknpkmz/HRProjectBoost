using AutoMapper;
using HRProjectBoost.DTOs.DTOs.Advance;
using HRProjectBoost.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Business.Mappings.AutoMapper
{
    public class AdvanceProfile : Profile
    {
        public AdvanceProfile()
        {
            CreateMap<Advance, AdvanceCreateDto>().ReverseMap();
            CreateMap<Advance, AdvanceDto>().ReverseMap();
        }
    }
}
