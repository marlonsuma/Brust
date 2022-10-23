using AutoMapper;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System.API.Models
{
    public class AddressModel
    {
        public string Url { get; set; }
        public string DonorId { get; set; }
        public string DonorUrl { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }

    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressModel>()
                .ForMember(m => m.Url, opt => opt.MapFrom<AddressUrlResolver>())
                .ForMember(m => m.DonorUrl, opt => opt.MapFrom<AddressDonorUrlResolver>());
        }
    }

    public class AddressModelProfile : Profile
    {
        public AddressModelProfile()
        {
            CreateMap<AddressModel, Address>()
                .ForMember(a => a.DonorId, opt => opt.Ignore())
                .ForMember(a => a.AddressType, opt => opt.Ignore());
        }
    }
}