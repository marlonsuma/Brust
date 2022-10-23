using System;
using System.Configuration;
using DonorTracking.Data;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(NiQ_Donor_Tracking_System.OwinStartup))]

namespace NiQ_Donor_Tracking_System
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.CreatePerOwinContext<NiqUserManager>(() => DependencyResolver.Current.GetService<NiqUserManager>());
            
            app.CreatePerOwinContext(() => new RepositoryConfigurationProvider(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ConnectionString));
            app.CreatePerOwinContext<NiqUserStore>((opt, cont) => new NiqUserStore(cont.Get<RepositoryConfigurationProvider>()));
            app.CreatePerOwinContext<NiqUserManager>((opt, cont) => new NiqUserManager(cont.Get<NiqUserStore>()));
            app.CreatePerOwinContext<NiqSignInManager>((opt, cont) => new NiqSignInManager(cont.Get<NiqUserManager>(), cont.Authentication));

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(4),
                AllowInsecureHttp = true
            });

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions { });

            //app.UseCookieAuthentication(new CookieAuthenticationOptions()
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/frmLogin.aspx"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        OnApplyRedirect = ctx =>
            //            {
            //                if (!ctx.Request.Uri.ToString().ToLower().Contains("api/"))
            //                {
            //                    ctx.Response.Redirect(ctx.RedirectUri);
            //                }
            //            }

            //    }
            //});

        }
    }
}
