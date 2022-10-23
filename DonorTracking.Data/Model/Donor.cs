using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    [Table("tblDonors")]
    public class Donor
    {
        public Donor()
        {
            MilkKits = new List<MilkKit>();
            BloodKits = new List<BloodKit>();
            LabKits = new List<LabKit>();
            Active = true;
        }
        public int Id { get; set; }
        public string DonorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public bool ReceiveConsentForm { get; set; }
        public bool ReceiveFinancialForm { get; set; }
        public DateTime? InactiveDate { get; set; }
        public string InactiveReason { get; set; }
        public string Notes { get; set; }
        
        [Computed]
        [Write(false)]
        public bool Active { get; set; }

        [Write(false)]
        public List<MilkKit> MilkKits { get; set; }

        [Write(false)]
        public List<BloodKit> BloodKits { get; set; }

        [Write(false)]
        public List<LabKit> LabKits { get; set; }

        [Write(false)]
        public Address ShippingAddress { get; set; }
        
        [Write(false)]
        public Address MailingAddress { get; set; }
    }
}