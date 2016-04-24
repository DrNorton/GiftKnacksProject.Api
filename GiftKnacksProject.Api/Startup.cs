using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Hosting;
using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FamilyTasks.Api;
using GiftKnacksProject.Api.App_Start;
using GiftKnacksProject.Api.Dependencies;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Swashbuckle.Application;

namespace GiftKnacksProject.Api
{
    public class Startup
    {
        private static readonly Lazy<CorsOptions> SignalrCorsOptions = new Lazy<CorsOptions>(() =>
        {
            return new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context =>
                    {
                        var policy = new CorsPolicy();
                        policy.AllowAnyOrigin = true;
                        policy.AllowAnyMethod = true;
                        policy.AllowAnyHeader = true;
                        policy.SupportsCredentials = true;
                        return Task.FromResult(policy);
                    }
                }
            };
        });

        public void Configuration(IAppBuilder app)
        {
            HookConnectionStrings();
            var config = new HttpConfiguration();
            var container = ConfigureWindsor(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(
                c =>
                {
                    AuthConfig.Register(app, container);
                    WebApiConfig.Register(c, container);
                    SwaggerConfig.Register(c);
                }
                );

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            app.Map("/signalr", map =>
            {
                map.UseCors(SignalrCorsOptions.Value);
                map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
                {
                    Provider = new QueryStringOAuthBearerProvider()
                });

                var hubConfiguration = new HubConfiguration
                {
                    Resolver = GlobalHost.DependencyResolver,
                    EnableJSONP = true,
                    
                };
                map.RunSignalR(hubConfiguration);
            });
           
        }

        

        private void HookConnectionStrings()
        {
            var fieldInfo = typeof(ConfigurationElementCollection).GetField("bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(ConfigurationManager.ConnectionStrings, false);

                // Check for AppSetting and Create
                if (ConfigurationManager.AppSettings["giftKnacksConnectionString"] != null)
                {
                    ConnectionStringSettings myDB = new ConnectionStringSettings("giftKnacksConnectionString", ConfigurationManager.AppSettings["giftKnacksConnectionString"]);
                    myDB.ProviderName = "System.Data.EntityClient";
                    ConfigurationManager.ConnectionStrings.Add(myDB);
                }
            }
        }

        public static IWindsorContainer ConfigureWindsor(HttpConfiguration configuration)
        {
            var container = CastleInstaller.Install();
           
            var dependencyResolver = new WindsorDependencyResolver(container);
            configuration.DependencyResolver = dependencyResolver;

            return container;
        }    
    }

  
    public class QueryStringOAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Query.Get("access_token");

            if (!string.IsNullOrEmpty(value))
            {
                context.Token = value;
            }

            return Task.FromResult<object>(null);
        }
    }

}