using System;
using System.Xml.Serialization;
using AutoMapper;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System.API.Models
{
    [XmlType("BloodKit")]
    public class BloodKitModel
    {
        public bool Active { get; set; }

        public string Din { get; set; }

        public string DonorId { get; set; }
        public string DonorUrl { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? ReceiveDate { get; set; }

        public string ShippingService { get; set; }

        public bool? Status { get; set; }

        public string TrackingNumber { get; set; }
        public string Url { get; set; }
    }

    public class BloodKitProfile : Profile
    {
        public BloodKitProfile()
        {
            CreateMap<BloodKit, BloodKitModel>()
                .ForMember(m => m.Url, opt => opt.MapFrom<BloodKitUrlResolver>())
                .ForMember(m => m.DonorUrl, opt => opt.MapFrom<BloodKitDonorUrlResolver>());
        }
    }

    public class BloodKitModelProfile : Profile
    {
        public BloodKitModelProfile()
        {
            CreateMap<BloodKitModel, BloodKit>()
                .ForMember(k => k.OrderDate, opt => opt.Ignore())
                .ForMember(k => k.Din, opt => opt.Ignore());
        }
    }
}