using Dapper.Contrib.Extensions;

namespace DonorTracking.Data {
    [Table("tblPrinters")]
    public class Printer
    {
        public int Id { get; set; }
        public string PrinterName { get; set; }
        public string PrinterIp { get; set; }
    }
}