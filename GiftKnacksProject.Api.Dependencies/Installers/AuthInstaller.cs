using System;
using System.Configuration;
using System.Web.Security;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GiftKnacksProject.Api.Controllers;
using GiftKnacksProject.Api.Dao.AuthUsers;
using GiftKnacksProject.Api.Dao.Emails;
using GiftKnacksProject.Api.Dao.Emails.Mailers;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.AuthUsers;
using GiftKnacksProject.Api.Helpers.Utils;
using GiftKnacksProject.Api.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;

namespace GiftKnacksProject.Api.Dependencies.Installers
{
    public class AuthInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var provider = new MachineKeyProtectionProvider();

            IDataProtector protector = provider.Create("ResetPasswordPurpose");
            container.Register(Component.For<IDataProtector>().Instance(protector));
            container.Register(
                Component.For<DataProtectorTokenProvider<ApplicationUser, long>>().LifestyleTransient());

            container.Register(Component.For<IIdentityMessageService>().ImplementedBy<AuthEmailService>().LifestyleTransient());
            container.Register(Component.For<IPasswordHasher>().ImplementedBy<PasswordHasher>().LifestyleTransient());
            container.Register(Component.For<IUserMailer>().ImplementedBy<UserMailer>().LifestyleTransient());

            container.Register(
              Component.For<IUserStore<ApplicationUser, long>>().ImplementedBy<CustomUserStore>().LifestyleTransient());

            container.Register(Component.For<CustomUserManager>().LifestyleTransient());
            container.Register(
                Component.For<UrlSettings>()
                    .Instance(new UrlSettings()
                    {
                        ApiUrl = ConfigurationManager.AppSettings["ApiUrl"],
                        SiteUrl = ConfigurationManager.AppSettings["SiteUrl"],
                        StorageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"]
                    }));

         
            container.Register(
                Component.For<OAuthAuthorizationServerOptions>()
                    .UsingFactoryMethod((kernel, parameters) => new OAuthAuthorizationServerOptions
                    {
                        AllowInsecureHttp = true,
                        TokenEndpointPath = new PathString("/api/token"),
                        AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                        Provider = new SimpleAuthorizationServerProvider(container)
                    })
                    .LifestyleTransient());
        }
    }

    public class MachineKeyProtectionProvider : IDataProtectionProvider
{
    public IDataProtector Create(params string[] purposes)
    {
        return new MachineKeyDataProtector(purposes);
    }
}

public class MachineKeyDataProtector : IDataProtector
{
    private readonly string[] _purposes;

    public MachineKeyDataProtector(string[] purposes)
    {
        _purposes = purposes;
    }

    public byte[] Protect(byte[] userData)
    {
        return MachineKey.Protect(userData, _purposes);
    }

    public byte[] Unprotect(byte[] protectedData)
    {
        return MachineKey.Unprotect(protectedData, _purposes);
    }
}
}
