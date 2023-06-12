using FluentValidation;
using HRProjectBoost.DTOs.DTOs.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Business.FluentValidations
{
    public class ManagerUpdateDtoValidator:AbstractValidator<ManagerUpdateDto>
    {
        public ManagerUpdateDtoValidator()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.").Matches(@"^[0-9]{12}$").WithMessage("Invalid phone number format. Please enter a 12-digit number.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.").MinimumLength(5).WithMessage("Address must be at least 5 characters long.").MaximumLength(100).WithMessage("Address cannot exceed 100 characters.");
        }
    }
}
