//using AutoMapper;
//using HRProjectBoost.DTOs.DTOs.Authentication;
//using HRProjectBoost.DTOs.DTOs.Manager;
//using HRProjectBoost.Entities.Domains;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace HRProjectBoost.UI.ViewComponents.Manager
//{
//    public class _HeaderProfileInformations: ViewComponent
//    {
//        private readonly UserManager<AppUser> userManager;
//        private readonly IMapper _mapper;

//        public _HeaderProfileInformations(UserManager<AppUser> userManager, IMapper mapper)
//        {
//            this.userManager = userManager;
//            _mapper = mapper;
//        }

//        public async Task<IViewComponentResult> InvokeAsync()
//        {
//            var datas = await userManager.FindByNameAsync(this.User.Identity.Name);
//            ViewBag.Name = datas.Name;
//            ViewBag.LastName = datas.LastName;
//            ViewBag.Job = datas.Job;
//            ViewBag.Photo = datas.ProfilePicture;
//            return View();
//        }
//    }
//}
