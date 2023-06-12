using HRProjectBoost.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Entities.Domains
{
    public class Advance
    {
        public int AdvanceId { get; set; }
        public int Total { get; set; }
        public string Description { get; set; }
        public AdvanceType AdvanceType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public AdvanceStatus AdvanceStatus { get; set; }
        public Status Status { get; set; }
        public DateTime AdvanceCreatedTime { get; set; } = DateTime.Parse(DateTime.UtcNow.ToString("d"));
        public DateTime AdvanceAnsweredTime { get; set; } = DateTime.Parse(DateTime.UtcNow.ToString("d"));



        //Nav Props
        public virtual AppUser AppUser { get; set; }
        public int AppUserId { get; set; }

        
    }
}
