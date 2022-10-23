using System.Configuration;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using DonorTracking.Data;
using Microsoft.Owin.Security;
using NiQ_Donor_Tracking_System.API.Models;

namespace NiQ_Donor_Tracking_System
{
    public static class ContainerSetup
    {
        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register(ctx => new RepositoryConfigurationProvider(ConfigurationManager.ConnectionStrings["NiQ_DonorTracking"].ConnectionString))
                   .As<IRepositoryConfigurationProvider>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterMapping();
            builder.RegisterDataRepositories();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();
            builder.RegisterIdentity();

            return builder.Build();
        }

        public static void RegisterMapping(this ContainerBuilder builder)
        {
            builder.RegisterType<AddressProfile>().As<Profile>();
            builder.RegisterType<AddressModelProfile>().As<Profile>();
            builder.RegisterType<BloodKitProfile>().As<Profile>();
            builder.RegisterType<BloodKitModelProfile>().As<Profile>();
            builder.RegisterType<MilkKitProfile>().As<Profile>();
            builder.RegisterType<MilkKitModelProfile>().As<Profile>();
            builder.RegisterType<LotProfile>().As<Profile>();
            builder.RegisterType<DonorProfile>().As<Profile>();
            builder.RegisterType<DonorDetailProfile>().As<Profile>();
            builder.RegisterType<DonorModelProfile>().As<Profile>();
            builder.RegisterModule<AutoMapperModule>();
        }
    }
}