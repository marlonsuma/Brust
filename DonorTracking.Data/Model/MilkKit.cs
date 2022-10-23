using System;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    [Table("tblMilkKits")]
    public class MilkKit
    {
        public MilkKit()
        {
            Active = true;
        }
        public int Id { get; set; }
        public string DonorId { get; set; }
        public int? LotId { get; set; }
        public string Barcode { get; set; }
        public string ShippingService { get; set; }
        public string TrackingNumber { get; set; }
        public string Volume { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? QuarantineDate { get; set; }
        public bool? Dnatest { get; set; }
        public bool? DrugAlcoholTest { get; set; }
        public bool? MicrobialTest { get; set; }
        public string Pallet { get; set; }

        public int? Grade { get; set; }
        public int? APC { get; set; }
        public int? EB { get; set; }

        public int? CC { get; set; }

        public int? RYM { get; set; }

        public bool? Mold { get; set; }

        public bool? STX { get; set; }

        public bool? ECOLI { get; set; }

        public bool? SAL { get; set; }
        public bool Active { get; set; }
        public DateTime? DatePaid { get; set; }
        public DateTime? Finalized { get; set; }

        [Write(false)]
        public Lot Lot { get; set; }

        [Write(false)]
        public string LotBarcode { get; set; }
    }
}