using FluentValidation;
using HRProjectBoost.DTOs.DTOs.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Business.FluentValidations
{
    public class CreatePersonnelDtoValidator : AbstractValidator<PersonnelCreateDTO>
    {
        public CreatePersonnelDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Lastname cannot be empty.");
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Birth date cannot be empty.");
            RuleFor(x => x.BirthCity).NotEmpty().WithMessage("Birth city cannot be empty.");
            RuleFor(x => x.IdentityNumber).NotEmpty().WithMessage("Identity number cannot be empty.");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start date cannot be empty.");
            RuleFor(x => x.Job).NotEmpty().WithMessage("Job cannot be empty.");
            RuleFor(x => x.Department).NotEmpty().WithMessage("Department cannot be empty.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.").Matches(@"^[0-9]{10}$").WithMessage("Invalid phone number format. Please enter a 10-digit number.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.").MinimumLength(5).WithMessage("Address must be at least 5 characters long.").MaximumLength(100).WithMessage("Address cannot exceed 100 characters.");
            RuleFor(x => x.Salary).NotEmpty().WithMessage("Salary cannot be empty.");


        }
    }
}
