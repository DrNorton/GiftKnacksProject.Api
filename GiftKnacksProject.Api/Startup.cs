using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FamilyTasks.Api;
using GiftKnacksProject.Api.Dependencies;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace GiftKnacksProject.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var container = ConfigureWindsor(GlobalConfiguration.Configuration);
            ConfigureOAuth(app, container);
            GlobalConfiguration.Configure(c => WebApiConfig.Register(c, container));
           
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

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


   


        public void ConfigureOAuth(IAppBuilder app, IWindsorContainer container)
        {
            //// Token Generation
            app.UseOAuthAuthorizationServer(container.Resolve<OAuthAuthorizationServerOptions>());
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

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