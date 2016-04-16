using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using GiftKnacksProject.Api.Services;
using GiftKnacksProject.Api.Services.Interfaces;
using GiftKnacksProject.Api.Services.Services;
using GiftKnacksProject.Api.Services.Services.ChatMessages;
using GiftKnacksProject.Api.Services.Services.FeedService;
using GiftKnacksProject.Api.Services.Storages;
using Microsoft.Azure.Documents.Client;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;


namespace GiftKnacksProject.Api.Dependencies.Installers
{
    public class ServiceInstaller:IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            var endpointUrl = ConfigurationManager.AppSettings["DocumentDbEndpointUrl"];
            var authorizationKey = ConfigurationManager.AppSettings["DocumentDbAuthorizationKey"];
            container.Register(
                Component.For<DocumentClient>().UsingFactoryMethod((kernel,parameters)=>new DocumentClient(new Uri(endpointUrl),authorizationKey)).LifestyleTransient());


            var fileService = new FileService(container.Resolve<UrlSettings>());
            container.Register(Component.For<IFileService>().Instance(fileService));
            var notificationConnectionQmString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.NotificationMQConnectionString"];
            var messageMqConnectionQmString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.MessagesMQConnectionString"];
            var databaseName = ConfigurationManager.AppSettings["DocumentDatabaseName"];
            container.Register(Component.For<INotificationService>().ImplementedBy<NotificationService>()
                .DependsOn(Dependency.OnValue("notififactionQueueClient",
                    QueueClient.CreateFromConnectionString(notificationConnectionQmString, "knackgiftnotifications")),
                    Dependency.OnValue("databasename", databaseName)).LifestyleTransient());
            container.Register(Component.For<IChatMessageService>().ImplementedBy<ChatMessageService>()
              .DependsOn(Dependency.OnValue("chatQueueClient", QueueClient.CreateFromConnectionString(messageMqConnectionQmString, "knackgiftmessages")),Dependency.OnValue("databaseName", databaseName)).LifestyleTransient());
            container.Register(Component.For<IUserOnlineSignalService>().ImplementedBy<UserOnlineSignalService>().LifeStyle.Singleton.Start());
        }
    }
}
