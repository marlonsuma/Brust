using System;
using AutoMapper;
using DonorTracking.Data;

namespace NiQ_Donor_Tracking_System.API.Models
{
    public class LotModel
    {
        public string Barcode { get; set; }
        public DateTime? BestByDate { get; set; }
        public bool Closed { get; set; }
        public bool Transferred { get; set; }
        public int? TotalCases { get; set; }
        public int? CasesRemaining { get; set; }
        public int? SamplePouches { get; set; }
    }

    public class LotProfile : Profile
    {
        public LotProfile()
        {
            CreateMap<Lot, LotModel>();
        }
    }
}