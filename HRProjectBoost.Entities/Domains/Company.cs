using HRProjectBoost.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Entities.Domains
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyTitle { get; set; }
        public string MersisNo { get; set; }
        public string TaxNo { get; set; }
        public string TaxAdministration { get; set; }
        public byte[]? Logo { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public int? PersonnelCount { get; set; }
        public DateTime EstablishDate { get; set; }
        public DateTime AgreementStartDate { get; set; }
        public DateTime AgreementEndDate { get; set; }
        public CompanyStatus CompanyStatus { get; set; }


        //Nav Props
        public ICollection<AppUser>? AppUser { get; set; }
    }
}
