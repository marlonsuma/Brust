using System;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    [Table("tblLots")]
    public class Lot
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public DateTime? BestByDate { get; set; }
        public bool Closed { get; set; }
        public bool Transferred { get; set; }
        public int? TotalCases { get; set; }
        public int? CasesRemaining { get; set; }
        public int? SamplePouches { get; set; }
    }
}