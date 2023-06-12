using AutoMapper;
using HRProjectBoost.Business.FluentValidations;
using HRProjectBoost.DataAccess.Context;
using HRProjectBoost.DTOs.DTOs.Advance;
using HRProjectBoost.DTOs.DTOs.Allowance;
using HRProjectBoost.DTOs.DTOs.Personnel;
using HRProjectBoost.Entities.Domains;
using HRProjectBoost.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRProjectBoost.UI.Areas.Personnel.Controllers
{
    [AllowAnonymous]
    [Area("Personnel")]
    public class PersonnelController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly HRProjectContext _context;
        private readonly IMapper _mapper;

        public PersonnelController(UserManager<AppUser> userManager, HRProjectContext context, IMapper mapper, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _userManager.FindByNameAsync(this.User.Identity.Name);
            TempData["company"] = datas.CompanyInfo;
            return View(_mapper.Map<PersonnelDTO>(datas));
        }

        [HttpGet]
        public async Task<IActionResult> Details(PersonnelDetailsDTO dto)
        {
            var datas = await _userManager.FindByNameAsync(this.User.Identity.Name);
            return View(_mapper.Map<PersonnelDetailsDTO>(datas));
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var datas = await _userManager.FindByNameAsync(this.User.Identity.Name);
            return View(_mapper.Map<PersonnelUpdateDTO>(datas));
        }

        [HttpPost]
        public async Task<object> Update(PersonnelUpdateDTO dto)
        {
            var user = await _userManager.FindByNameAsync(this.User.Identity.Name);
            var UpdateValidator = new PersonnelUpdateDtoValidator();
            var validatorResult = UpdateValidator.Validate(dto);
            if (ModelState.IsValid && validatorResult.IsValid)
            {
                var files = Request.Form.Files;

                if (files.Count != 0)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        files[0].CopyTo(memoryStream);
                        user.ProfilePicture = memoryStream.ToArray();
                    }
                }

                user.Address = dto.Address;
                user.PhoneNumber = dto.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                _context.SaveChanges();

                if (result.Succeeded)
                    return RedirectToAction("Index", "Manager");
                else
                    foreach (var item in result.Errors)
                        ModelState.AddModelError("", item.Description);
            }
            else
            {
                //ModelState.AddModelError("", "Hatalar oluştu.");

                string errorMessage = string.Join(", ", validatorResult.Errors.Select(error => error.ErrorMessage));
                ViewBag.ErrorMessage = errorMessage; // Hata alabiliyoruz ancak msji ekrana veremedik henuz viewbag yerine Toastr kurulacak...
                return RedirectToAction("Update", dto);
            }

            return View();
        }

        public IActionResult CreateAllowance()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAllowance(AllowanceCreateDto allowanceCreateDto)
        {
            var ext = Path.GetExtension(allowanceCreateDto.AllowanceFile.FileName);
            var path = Directory.GetCurrentDirectory() + "/wwwroot/pdfs/" + Guid.NewGuid().ToString() + ext;

            if (ModelState.IsValid)
            {
                if (allowanceCreateDto.AllowanceFile.ContentType == "image/jpeg" || allowanceCreateDto.AllowanceFile.ContentType == "application/pdf")
                {
                    FileStream stream = new FileStream(path, FileMode.Create);
                    await allowanceCreateDto.AllowanceFile.CopyToAsync(stream);
                }
                else
                    ModelState.AddModelError("", "Wrong Format.Please use only pdf or jpeg.");

                Allowance allowance = new();
                allowance.AllowanceStatus = allowanceCreateDto.AllowanceStatus;
                allowance.AllowanceType = allowanceCreateDto.AllowanceType;
                allowance.CurrencyType = allowanceCreateDto.CurrencyType;
                allowance.AllowanceCreatedTime = allowanceCreateDto.AllowanceCreatedTime;
                allowance.Total = allowanceCreateDto.Total;
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                allowance.AppUserId = user.Id;
                allowance.Path = path.Substring(path.Length - 40);

                _context.Allowance.Add(allowance);
                await _context.SaveChangesAsync();
            }
            return View(allowanceCreateDto);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PersonnelChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordDto);
            }
            var user = await _userManager.GetUserAsync(User);

            if (user is not null)
            {
                var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (result.Succeeded)
                {
                    user.Password = changePasswordDto.NewPassword;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Logout","User", new { area = "" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
                ModelState.AddModelError("", "User cannot be found");

            return RedirectToAction("Logout", "User");
        }

        [HttpGet]
        public IActionResult CreateAdvance()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvance(AdvanceCreateDto advanceCreateDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            if (ModelState.IsValid)
            {
                Advance advance = new()
                {
                    AdvanceType = advanceCreateDto.AdvanceType,
                    AdvanceStatus = advanceCreateDto.AdvanceStatus,
                    CurrencyType = advanceCreateDto.CurrencyType,
                    Description = advanceCreateDto.Description,
                    Total = advanceCreateDto.Total,
                    Status = advanceCreateDto.Status,
                    AppUser = user,
                    AppUserId = user.Id,
                    AdvanceCreatedTime = DateTime.Parse(DateTime.UtcNow.ToString("d")),
                    AdvanceAnsweredTime = DateTime.Parse(DateTime.UtcNow.ToString("d"))
                };

                if (advanceCreateDto.AdvanceType == AdvanceType.Individiual && advanceCreateDto.Total > user.Salary * 3)
                    ModelState.AddModelError("", "The total cannot be more than 3 times your salary ");
                else if (advanceCreateDto.CurrencyType == CurrencyType.USD && advanceCreateDto.Total > 5000)
                    ModelState.AddModelError("", "The total cannot be more than 5000 USD ");
                else if (advanceCreateDto.CurrencyType == CurrencyType.EUR && advanceCreateDto.Total > 5000)
                    ModelState.AddModelError("", "The total cannot be more than 5000 EUR ");
                else
                {
                    await _context.Advance.AddAsync(advance);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AdvanceList");
                }
            }
            else
                ModelState.AddModelError("", "Error");

            return View();
        }

        [HttpGet]
        public IActionResult AdvanceList(AdvanceDto advanceDto)
        {
            List<Advance> advanceList = _context.Advance.Include(x => x.AppUser).ToList();
            var dto = _mapper.Map<List<AdvanceDto>>(advanceList);
            return View(dto);
        }
    }
}
