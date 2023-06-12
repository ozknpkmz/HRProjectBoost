using AutoMapper;
using HRProjectBoost.DTOs.DTOs.Manager;
using HRProjectBoost.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Business.Mappings.AutoMapper
{
    public class ManagerProfile : Profile
    {
        public ManagerProfile()
        {
            CreateMap<AppUser, ManagerDto>().ReverseMap();
            CreateMap<AppUser, ManagerUpdateDto>().ReverseMap();
            CreateMap<AppUser, ManagerDetailsDto>().ReverseMap();
        }
    }
}
