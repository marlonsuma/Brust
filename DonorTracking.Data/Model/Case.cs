using System;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    [Table("tblCases")]
    public class Case
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public int? LotId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? ShipDate { get; set; }
        public string ShippingService { get; set; }
        public string TrackingNumber { get; set; }
        public string PoNumber { get; set; }
        public int? CaseQuantity { get; set; }
        public bool Active { get; set; }

        [Write(false)]
        public string Location { get; set; }

        [Write(false)]
        public string LotBarcode { get; set; }
    }
}