using System.Web.Routing;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using GiftKnacksProject.Api.Dependencies.Resolver;
using Microsoft.AspNet.SignalR;

namespace GiftKnacksProject.Api.Dependencies
{
    public class CastleInstaller
    {
        public static IWindsorContainer Install()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            var signalrDependency = new SignalrDependencyResolver(container.Kernel);
            GlobalHost.DependencyResolver = signalrDependency;
            return container;
        }
    }
}
