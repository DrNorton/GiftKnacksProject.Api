using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KatanaContrib.Security.VK;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;

namespace GiftKnacksProject.Api.Controllers
{
    public static class AuthSettings
    {
        public static VkAuthenticationOptions vkAuthOptions { get; set; }
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get;  set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get;  set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get;  set; }
    }
}
