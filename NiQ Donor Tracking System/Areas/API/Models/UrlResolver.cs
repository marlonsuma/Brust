using System.Text;
using System.Web.Http.Routing;
using AutoMapper;
using DonorTracking.Data;
using NiQ_Donor_Tracking_System.API.Controllers;
using NiQ_Donor_Tracking_System.API.Models;

namespace NiQ_Donor_Tracking_System.API
{
    public class AddressDonorUrlResolver : IValueResolver<Address, AddressModel, string>
    {
        public string Resolve(Address source, AddressModel destination, string destMember, ResolutionContext context)
        {
            UrlHelper helper = (UrlHelper)context.Items["Url"];
            return helper.Link(DonorController.RequestRoute, new { donorId = source.DonorId });
        }
    }

    public class AddressUrlResolver : IValueResolver<Address, AddressModel, string>
    {
        public string Resolve(Address source, AddressModel destination, string destMember, ResolutionContext context)
        {
            UrlHelper helper = (UrlHelper)context.Items["Url"];
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(helper.Link(DonorController.RequestRoute, new { donorId = source.DonorId }));
            urlBuilder.Append("/");
            urlBuilder.Append(source.AddressType == AddressType.Mailing ? "mailingaddress" : "shippingaddress");
            return urlBuilder.ToString();
        }
    }

    public class BloodKitUrlResolver : IValueResolver<BloodKit, BloodKitModel, string>
    {
        public string Resolve(BloodKit source, BloodKitModel destination, string destMember, ResolutionContext context)
        {
            UrlHelper helper = (UrlHelper) context.Items["Url"];

            return helper.Link(BloodKitController.RequestRoute, new { din = source.Din });
        }
    }

    public class BloodKitDonorUrlResolver : IValueResolver<BloodKit, BloodKitModel, string>
    {
        public string Resolve(BloodKit source, BloodKitModel destination, string destMember, ResolutionContext context)
        {
            UrlHelper helper = (UrlHelper)context.Items["Url"];

            return helper.Link(DonorController.RequestRoute, new { donorId = source.DonorId });
        }
    }

    public class DonorUrlResolver : IValueResolver<Donor, DonorModel, string>
    {
        public string Resolve(Donor source, DonorModel destination, string destMember, ResolutionContext context)
        {
            UrlHelper helper = (UrlHelper)context.Items["Url"];

            return helper.Link(DonorController.RequestRoute, new { donorId = source.DonorId });
        }
    }

    public class MilkKitUrlResolver : IValueResolver<MilkKit, MilkKitModel, string>
    {
        public string Resolve(MilkKit source, MilkKitModel destination, string destMember, ResolutionContext context)
        {
            UrlHelper helper = (UrlHelper)context.Items["Url"];

            return helper.Link(MilkKitController.RequestRoute, new { barcode = source.Barcode });
        }
    }

    public class MilkKitDonorUrlResolver : IValueResolver<MilkKit, MilkKitModel, string>
    {
        public string Resolve(MilkKit source, MilkKitModel destination, string destMember, ResolutionContext context)
        {
            UrlHelper helper = (UrlHelper)context.Items["Url"];

            return helper.Link(DonorController.RequestRoute, new { donorId = source.DonorId });
        }
    }
}