using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GiftKnackAgentCore.Services;
using GiftKnackMessageAgent.Services;
using Microsoft.Azure;
using Microsoft.Azure.Documents.Client;

namespace GiftKnackMessageAgent.Installers
{
    public class ServiceInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<Functions>().LifestyleTransient());
            container.Register(Component.For<IChatMessageFromMqProcessor>().ImplementedBy<ChatMessageFromMqProcessor>().LifestyleTransient());
            var endpointUrl = CloudConfigurationManager.GetSetting("DocumentDbEndpointUrl");
            var authorizationKey = CloudConfigurationManager.GetSetting("DocumentDbAuthorizationKey");
            container.Register(
                Component.For<DocumentClient>().UsingFactoryMethod((kernel, parameters) => new DocumentClient(new Uri(endpointUrl), authorizationKey)).LifestyleTransient());

            container.Register(Component.For<INoSqlDatabaseRepository>().ImplementedBy<NoSqlDatabaseRepository>().LifestyleTransient());
        }
    }
}
