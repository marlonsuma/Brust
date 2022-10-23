using System.Web.Http;
using AutoMapper;

namespace NiQ_Donor_Tracking_System.API.Controllers
{
   [ClaimsAuthorization(ClaimType = "ApiUser", ClaimValue = "1")]
    public class NiqController : ApiController
    {
        internal void MapOptions(IMappingOperationOptions opts)
        {
            opts.Items["Url"] = Url;
        }
    }
}