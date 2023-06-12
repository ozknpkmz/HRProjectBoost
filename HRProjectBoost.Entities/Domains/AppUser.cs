using HRProjectBoost.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace HRProjectBoost.Entities.Domains
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthCity { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CompanyInfo { get; set; } // bunu kaldirabiliriz.
        public string Job { get; set; }
        public Department Department { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public Status Status { get; set; } = Status.Active;
        public byte[]? ProfilePicture { get; set; }

        //Nav Props
        public virtual ICollection<Allowance>? Allowances { get; set; }
        public virtual ICollection<Advance>? Advances { get; set; }

        public int? CompanyId { get; set; }
        public Company? Company { get; set; }


    }
}
