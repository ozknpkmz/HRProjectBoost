using AutoMapper;
using HRProjectBoost.DTOs.DTOs.Personnel;
using HRProjectBoost.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Business.Mappings.AutoMapper
{
    public class PersonnelProfile : Profile
    {
        public PersonnelProfile()
        {
            CreateMap<AppUser,PersonnelCreateDTO>().ReverseMap();
            CreateMap<AppUser,PersonnelDTO>().ReverseMap();
            CreateMap<AppUser,PersonnelUpdateDTO>().ReverseMap();
            CreateMap<AppUser,PersonnelDetailsDTO>().ReverseMap();
            //CreateMap<AppUser,PersonnelChangePasswordDto>().ReverseMap();

        }
    }
}
