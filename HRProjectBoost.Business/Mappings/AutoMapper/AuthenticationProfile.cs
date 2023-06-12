using AutoMapper;
using HRProjectBoost.DTOs.DTOs.Authentication;
using HRProjectBoost.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Business.Mappings.AutoMapper
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<AppUser, LoginDto>().ReverseMap();
        }
    }
}
