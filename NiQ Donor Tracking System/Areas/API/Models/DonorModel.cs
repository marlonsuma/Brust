using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using AutoMapper;
using DonorTracking.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NiQ_Donor_Tracking_System.API.Models
{
    [XmlType("Donor")]
    public class DonorModel
    {
        public string Url { get; set; }
        public string DonorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public bool ReceiveConsentForm { get; set; }
        public bool ReceiveFinancialForm { get; set; }
        public DateTime? InactiveDate { get; set; }
        public string InactiveReason { get; set; }
        [ReadOnly(true)]
        public bool Active { get; set; }
        public string Notes { get; set; }
        public string MilkKitsUrl => Url + "/milkkit";
        public string BloodKitsUrl => Url + "/bloodkit";
        public string ShippingAddressUrl => Url + "/shippingaddress";
        public string MailingAddressUrl => Url + "/mailingaddress";

    }
    [XmlType("Donor")]
    public class DonorDetailModel : DonorModel
    {
        public List<BloodKitModel> BloodKits { get; set; }
        public List<MilkKitModel> MilkKits { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel MailingAddress { get; set; }
    }

    public class DonorProfile : Profile
    {
        public DonorProfile()
        {
            CreateMap<Donor, DonorModel>()
                .ForMember(m => m.Url, opt => opt.MapFrom<DonorUrlResolver>());
        }
    }

    public class DonorDetailProfile : Profile
    {
        public DonorDetailProfile()
        {
            CreateMap<Donor, DonorDetailModel>()
                .ForMember(m => m.Url, opt => opt.MapFrom<DonorUrlResolver>());
        }
    }

    public class DonorModelProfile : Profile
    {
        public DonorModelProfile()
        {
            CreateMap<DonorModel, Donor>()
                .ForMember(d => d.DonorId, opt => opt.Ignore());
        }
    }

    public class DateOnlyConverter : IsoDateTimeConverter
    {
        public DateOnlyConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}