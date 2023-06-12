using AutoMapper;
using HRProjectBoost.DTOs.DTOs.Admin;
using HRProjectBoost.DTOs.DTOs.Personnel;
using HRProjectBoost.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Business.Mappings.AutoMapper
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Company, CompanyCreateDto>().ReverseMap();
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<AppUser,ManagerCreateDto>().ReverseMap();

        }
    }
}
