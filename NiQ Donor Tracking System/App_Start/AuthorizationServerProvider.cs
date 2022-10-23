using System.Security.Claims;
using System.Threading.Tasks;
using DonorTracking.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;

namespace NiQ_Donor_Tracking_System
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(
            OAuthValidateClientAuthenticationContext context)
        {

            if (context.TryGetBasicCredentials(out string clientId, out string clientSecret))
            {
                NiqUserManager userManager = context.OwinContext.Get<NiqUserManager>();

                try
                {
                    User user = await userManager.FindAsync(clientId, clientSecret);

                    if (user != null)
                    {
                        // Client has been verified.
                        context.OwinContext.Set<User>("oauth:client", user);
                        context.Validated(clientId);
                    }
                    else
                    {
                        // Client could not be validated.
                        context.SetError("invalid_client", "Client credentials are invalid.");
                        context.Rejected();
                    }
                }
                catch
                {
                    // Could not get the client through the IClientManager implementation.
                    context.SetError("server_error");
                    context.Rejected();
                }
            }
            else
            {
                // The client credentials could not be retrieved.
                context.SetError(
                    "invalid_client",
                    "Client credentials could not be retrieved through the Authorization header.");

                context.Rejected();
            }
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            User client = context.OwinContext.Get<User>("oauth:client");
            NiqUserManager userManager = context.OwinContext.Get<NiqUserManager>();
            if (client != null)
            {
                ClaimsIdentity identity = await userManager.CreateIdentityAsync(
                    client,
                    DefaultAuthenticationTypes.ExternalBearer);
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Invalid User Id or Password");
            }
        }
    }
}