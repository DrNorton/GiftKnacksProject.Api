using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Castle.Windsor;
using GiftKnacksProject.Api.Controllers;
using GiftKnacksProject.Api.Providers;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace GiftKnacksProject.Api.App_Start
{
    public partial class AuthConfig
    {

        public static void Register(IAppBuilder app, IWindsorContainer container)
        {
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            AuthSettings.OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            //// Token Generation
            app.UseOAuthAuthorizationServer(container.Resolve<OAuthAuthorizationServerOptions>());
            app.UseOAuthBearerAuthentication(AuthSettings.OAuthBearerOptions);
            var googleOAuth2AuthenticationOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "433960127018-5jhtpcdbbjj757bf3tp91m6ooircae79.apps.googleusercontent.com",
                ClientSecret = "57jNa_m8qV-jQq2oLL5dmPGr",
                Scope = { "email"},
                Provider = new GoogleAuthProvider()
            };
            AuthSettings.googleAuthOptions = googleOAuth2AuthenticationOptions;
            app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions);
            var facebook = new FacebookAuthenticationOptions()
            {
                AppId = "235373570153403",
                AppSecret = "a170501d7451e542472856df08ce3750",
                Scope = { "email", "public_profile" },
                Provider = new FacebookAuthProvider()
            };
            AuthSettings.facebookAuthOptions = facebook;
            app.UseFacebookAuthentication(facebook);
        }
    }
}