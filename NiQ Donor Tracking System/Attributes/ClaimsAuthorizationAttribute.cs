using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NiQ_Donor_Tracking_System
{
    public class ClaimsAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            //#region OLD stuff comment out for now
            ////// MLS 10242021
            ////if (principal == null || !principal.Identity.IsAuthenticated)
            ////{
            ////    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK);

            ////    return Task.FromResult<object>(null);
            ////}

            ////if (!(principal.HasClaim(c => c.Type == ClaimType && c.Value == ClaimValue)))
            ////{
            ////    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK);

            ////    return Task.FromResult<object>(null);
            ////}
            //#endregion

            /*******************************************************************************************/
            ///* Start New Code */
            //grab NiQ_IserName, NiQ_Password, NiQ_R@ndo from web.config
            var NiQ_UserName = System.Configuration.ConfigurationManager.AppSettings["NiQ_UserName"];
            var NiQ_Password = System.Configuration.ConfigurationManager.AppSettings["NiQ_Password"];
            var NiQ_Rando = System.Configuration.ConfigurationManager.AppSettings["NiQ_Rando"];
            var NiQ_AllowedIpsRaw = System.Configuration.ConfigurationManager.AppSettings["NiQ_AllowedIps"];

            // grab stuff from header
            var ctx = System.Web.HttpContext.Current.Request;
            var _NiQ_UserName = ctx.Headers["NiQ_UserName"];
            var _NiQ_Password = ctx.Headers["NiQ_Password"];
            var _NiQ_Rando = ctx.Headers["NiQ_Rando"];
            var _NiQ_Ip = ctx.ServerVariables["REMOTE_ADDR"];
            bool IpIsAllowed = false;
            if (NiQ_AllowedIpsRaw.ToString().ToLower().Contains("allow-all"))
            { 
                IpIsAllowed = true; 
            }
            else
            {
                string IpCheckString = "," + _NiQ_Ip + ",";
                IpIsAllowed = NiQ_AllowedIpsRaw.Contains(IpCheckString) ? true : false;
            }
            if (NiQ_UserName == _NiQ_UserName && NiQ_Password == _NiQ_Password && NiQ_Rando == _NiQ_Rando  && IpIsAllowed)
            {
                return Task.FromResult<object>(null);
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            //return new Task<>();
            /** END NEW CODE **/
            return Task.FromResult<object>(null);
        }
    }
}