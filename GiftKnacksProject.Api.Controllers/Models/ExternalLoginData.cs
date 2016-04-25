using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace GiftKnacksProject.Api.Controllers.Models
{
    public class ExternalLoginData
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserName { get; set; }
        public string ExternalAccessToken { get; set; }

        public string Email { get; set; }

        private static async Task<FacebookUserViewModel> GetEmailFromFacebook(string token)
        {
            FacebookUserViewModel fbUser = null;
            var path = "https://graph.facebook.com/me?fields=email&access_token=" + token;
            var client = new HttpClient();
            var uri = new Uri(path);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                fbUser = Newtonsoft.Json.JsonConvert.DeserializeObject<FacebookUserViewModel>(content);
            }
            return fbUser;
        }

        public static async Task<ExternalLoginData> FromIdentity(ClaimsIdentity identity,string provider)
        {
            if (identity == null)
            {
                return null;
            }

            Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
            {
                return null;
            }

            if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
            {
                return null;
            }
            var externalToken = identity.FindFirstValue("ExternalAccessToken");
            var email = identity.FindFirstValue(ClaimTypes.Email);
            if (provider == "Facebook")
            {
                var result=await GetEmailFromFacebook(externalToken);
                email = result.email;
            }

            return new ExternalLoginData
            {
                LoginProvider = providerKeyClaim.Issuer,
                ProviderKey = providerKeyClaim.Value,
                UserName = identity.FindFirstValue(ClaimTypes.Name),
                ExternalAccessToken = externalToken,
                Email = email
            };
        }
    }

    public class FacebookUserViewModel
    {
        public string email { get; set; }
    }

    
}
