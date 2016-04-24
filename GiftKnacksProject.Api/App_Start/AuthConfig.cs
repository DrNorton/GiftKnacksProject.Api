using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Castle.Windsor;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace GiftKnacksProject.Api.App_Start
{
    public partial class AuthConfig
    {
        public static void Register(IAppBuilder app, IWindsorContainer container)
        {
            //// Token Generation
            app.UseOAuthAuthorizationServer(container.Resolve<OAuthAuthorizationServerOptions>());
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        
        }
    }
}