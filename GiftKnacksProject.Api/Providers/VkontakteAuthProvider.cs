using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using KatanaContrib.Security.VK;

namespace GiftKnacksProject.Api.Providers
{
    public class VkontakteAuthProvider:VkAuthenticationProvider
    {
        
        public override Task Authenticated(VkAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }
    
    }
}