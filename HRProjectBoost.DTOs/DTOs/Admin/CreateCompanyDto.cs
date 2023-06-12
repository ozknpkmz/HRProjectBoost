using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.DTOs.DTOs.Admin
{
    public class CompanyCreateDto
    {
        public string CompanyName { get; set; }
        public string CompanyTitle { get; set; }
        public string MersisNo { get; set; }
        public string TaxNo { get; set; }
        public string TaxAdministration { get; set; }
        public byte[]? Logo { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyAddress { get; set; }
        public DateTime EstablishDate { get; set; }

    }
}
