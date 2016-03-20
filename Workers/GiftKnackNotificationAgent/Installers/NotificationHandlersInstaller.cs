using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GiftKnackNotificationAgent.Models;
using GiftKnackNotificationAgent.Models.Handlers;
using GiftKnackNotificationAgent.Services;

namespace GiftKnackNotificationAgent.Installers
{
    public class NotificationHandlersInstaller:IWindsorInstaller
    {
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.AddFacility<TypedFactoryFacility>()
                    .Register(
                        Classes.FromThisAssembly()
                            .BasedOn(typeof(BaseNotificationHandler<>))
                            .WithServiceAllInterfaces()
                            .LifestyleTransient(),
                        Component.For<INotificationHandlerFactory>().AsFactory());
                container.Register(
                    Component.For<IHandlersDispatcher>().ImplementedBy<HandlersDispatcher>().LifestyleTransient());
            container.Register(
            Component.For<IMessageFromMqProcessor>()
                .ImplementedBy<MessageFromMqProcessor>()
                .LifestyleTransient());
        }
        
    }
}
