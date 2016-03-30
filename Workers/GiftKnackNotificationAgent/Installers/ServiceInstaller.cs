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
using GiftKnackNotificationAgent.Services;
using Microsoft.Azure;
using Microsoft.Azure.Documents.Client;

namespace GiftKnackNotificationAgent.Installers
{
    public class ServiceInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var endpointUrl = CloudConfigurationManager.GetSetting("DocumentDbEndpointUrl");
            var authorizationKey = CloudConfigurationManager.GetSetting("DocumentDbAuthorizationKey");
            var databaseName = CloudConfigurationManager.GetSetting("DocumentDatabaseName");

            container.Register(
                Component.For<DocumentClient>()
                    .DependsOn(Dependency.OnValue("serviceEndpoint", new Uri(endpointUrl)),
                        Dependency.OnValue("authKeyOrResourceToken", authorizationKey))
                    .LifestyleTransient());

            container.Register(Component.For<Functions>().LifestyleTransient());
            var baseUrl = CloudConfigurationManager.GetSetting("ApiUrl");
            if (baseUrl == null)
            {
                throw new Exception("ApiUrl not exist");
            }
            container.Register(Component.For<IRealTimePushNotificationService>().ImplementedBy<RealTimePushNotificationService>().DependsOn(Dependency.OnValue("baseUrl", baseUrl)).LifestyleTransient());
            container.Register(Component.For<INoSqlDatabaseRepository>().ImplementedBy<NoSqlDatabaseRepository>()
         .DependsOn(Dependency.OnValue("client",
            new DocumentClient(new Uri(endpointUrl), authorizationKey)),
             Dependency.OnValue("databasename", databaseName)).LifestyleTransient());
        }
    }
}
