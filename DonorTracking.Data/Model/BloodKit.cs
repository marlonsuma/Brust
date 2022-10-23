using System;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    [Table("tblBloodKits")]
    public class BloodKit
    {
        public BloodKit()
        {
            Active = true;
        }
        public int Id { get; set; }
        public string DonorId { get; set; }
        public string Din { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string ShippingService { get; set; }
        public bool? Status { get; set; }
        public string TrackingNumber { get; set; }
        public bool? Active { get; set; }
    }
}