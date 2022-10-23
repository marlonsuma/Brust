using System;
using Autofac;

namespace DonorTracking.Data
{
    public static class Bootstrapper
    {
        public static void RegisterDataRepositories(this ContainerBuilder builder)
        {
            builder.RegisterType<AddressRepository>().As<IAddressRepository>();
            builder.RegisterType<BloodKitRepository>().As<IBloodKitRepository>();
            builder.RegisterType<CaseRepository>().As<ICaseRepository>();
            builder.RegisterType<DinGenerator>().As<IDinGenerator>();
            builder.RegisterType<TransactionLog>().As<ITransactionLog>();
            builder.RegisterType<DonorRepository>().As<IDonorRepository>();
            builder.RegisterType<LabKitRepository>().As<ILabKitRepository>();
            builder.RegisterType<LotRepository>().As<ILotRepository>();
            builder.RegisterType<PrinterRepository>().As<IPrinterRepository>();
            builder.RegisterType<MilkKitRepository>().As<IMilkKitRepository>();
        }

        public static void RegisterIdentity(this ContainerBuilder builder)
        {
            builder.RegisterType<NiqUserStore>().InstancePerRequest();
            builder.RegisterType<NiqUserManager>().InstancePerRequest();
            builder.RegisterType<NiqSignInManager>().InstancePerRequest();
        }
    }
}