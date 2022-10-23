using System;
using AutoMapper;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System.API.Models
{
    public class MilkKitModel
    {
        public bool Active { get; set; }
        public string Barcode { get; set; }
        public string DonorId { get; set; }
        public string DonorUrl { get; set; }
        public DateTime? Finalized { get; set; }
        public bool? GeneticTestResult { get; set; }
        public LotModel Lot { get; set; }
        public bool? MicrobialTestResult { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Pallet { get; set; }
        public DateTime? QuarantineDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string ShippingService { get; set; }
        public bool? ToxicologyTestResult { get; set; }
        public string TrackingNumber { get; set; }
        public string Volume { get; set; }
        public string Url {get; set; }
    }

    public class MilkKitProfile : Profile
    {
        public MilkKitProfile()
        {
            CreateMap<MilkKit, MilkKitModel>()
                .ForMember(m => m.Url, opt => opt.MapFrom<MilkKitUrlResolver>())
                .ForMember(m => m.DonorUrl, opt => opt.MapFrom<MilkKitDonorUrlResolver>())
                .ForMember(d => d.MicrobialTestResult, opt=> opt.MapFrom(s => s.MicrobialTest))
                .ForMember(d => d.ToxicologyTestResult, opt => opt.MapFrom(s => s.DrugAlcoholTest))
                .ForMember(d => d.GeneticTestResult, opt => opt.MapFrom(s => s.Dnatest))
                .ForMember(d => d.MicrobialTestResult, opt => opt.MapFrom(s => s.MicrobialTest))
                .ForMember(d => d.ToxicologyTestResult, opt => opt.MapFrom(s => s.DrugAlcoholTest))
                .ForMember(d => d.PaidDate, opt => opt.MapFrom(s => s.DatePaid));
        }
    }

    public class MilkKitModelProfile : Profile
    {
        public MilkKitModelProfile()
        {
            CreateMap<MilkKitModel, MilkKit>()
                .ForMember(k => k.Barcode, opt => opt.Ignore());
        }
    }

}