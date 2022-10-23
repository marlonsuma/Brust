using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    [Table("tblAddresses")]
    public class Address
    {
        public int Id { get; set; }
        public string DonorId { get; set; }
        public AddressType AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }

    public enum AddressType
    {
        Mailing = 0,
        Shipping = 1
    }
}