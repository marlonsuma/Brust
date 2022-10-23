using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.WebApi;
using AutoMapper;
using DonorTracking.Data;
using NiQ_Donor_Tracking_System.API.Models;

namespace NiQ_Donor_Tracking_System
{
    public class Global : HttpApplication, IContainerProviderAccessor
    {
        private static IContainerProvider _containerProvider;
        public IContainerProvider ContainerProvider => _containerProvider;

        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

        protected void Application_BeginRequest(object sender, EventArgs e) { }

        protected void Application_End(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e) { }

        protected void Application_Start(object sender, EventArgs e)
        {
            IContainer container = ContainerSetup.ConfigureContainer();

            _containerProvider = new ContainerProvider(container);
            
            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Session_End(object sender, EventArgs e) { }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["ui"] = new UserInfo();
        }

        public class PrinterInfo
        {
            public const int PORT = 9100;
        }
        public class UserInfo
        {
            public bool Administrator { get; set; } = false;

            public int ID { get; set; } = 0;

            public int PrinterID { get; set; } = 0;

            public string Username { get; set; } = "";
        }
    }
}