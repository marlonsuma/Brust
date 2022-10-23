using System;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    [Table("tblDNAKits")]
    public class LabKit
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string DonorId { get; set; }
        public int? MilkKitId { get; set; }
        public string ShippingService { get; set; }
        public string TrackingNumber { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? MicrobialOrdered { get; set; }
        public DateTime? GeneticOrdered { get; set; }
        public DateTime? ToxicologyOrdered { get; set; }
    }
}