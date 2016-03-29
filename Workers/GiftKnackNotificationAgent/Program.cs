using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace GiftKnackNotificationAgent
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        public static void Main()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());
         
            var config = new JobHostConfiguration() {JobActivator = new JobActivator(container) };
            config.UseServiceBus();

            var host = new JobHost(config);

            // The following code ensures that the WebJob will be running continuously
            DisplaySettings();
            host.RunAndBlock();
        }


        public static void DisplaySettings()
        {
            var list = new Dictionary<string, string>();
            list.Add("DocumentDbEndpointUrl", CloudConfigurationManager.GetSetting("DocumentDbEndpointUrl"));
            list.Add("DocumentDbAuthorizationKey", CloudConfigurationManager.GetSetting("DocumentDbAuthorizationKey"));
            list.Add("AzureWebJobsDashboard", CloudConfigurationManager.GetSetting("AzureWebJobsDashboard"));
            list.Add("AzureWebJobsStorage", CloudConfigurationManager.GetSetting("AzureWebJobsStorage"));
            list.Add("AzureWebJobsServiceBus", CloudConfigurationManager.GetSetting("AzureWebJobsServiceBus"));
            list.Add("giftKnacksConnectionString", CloudConfigurationManager.GetSetting("giftKnacksConnectionString"));
            var connectionString = ConfigurationManager.ConnectionStrings["giftKnacksConnectionString"];
            if (connectionString == null)
            {
                list.Add("connectionstring", "null");
            }
            else
            {
                list.Add("connectionstring", connectionString.ConnectionString);
                list.Add("ProviderName", connectionString.ProviderName);
            }



            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

    }
}
