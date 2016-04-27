using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Mvc;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Controllers.Models;
using GiftKnacksProject.Api.Dao.AuthUsers;
using GiftKnacksProject.Api.Dao.Emails.Mailers;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.AuthUsers;
using GiftKnacksProject.Api.Dto.Dtos;
using GiftKnacksProject.Api.Dto.Dtos.Profile;
using GiftKnacksProject.Api.EfDao;
using GiftKnacksProject.Api.EfDao.Base;
using GiftKnacksProject.Api.Services;
using GiftKnacksProject.Api.Services.Interfaces;
using GiftKnacksProject.Api.Services.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/Account")]
   
    public class AccountController : CustomApiController
    {
        private readonly CustomUserManager _userManager;
        private readonly IProfileRepository _profileRepository;
        private readonly UrlSettings _urlSettings;
        private readonly IFileService _fileService;
        private readonly IUserOnlineSignalService _userOnlineSignalService;
        private readonly INotificationService _notificationService;
        private readonly IUserMailer _mailer;
        private readonly IOAuthClientsRepository _oAuthClientsRepository;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public AccountController(CustomUserManager userManager, IProfileRepository profileRepository, UrlSettings urlSettings, IFileService fileService, IUserOnlineSignalService userOnlineSignalService, INotificationService notificationService,IUserMailer mailer,IOAuthClientsRepository oAuthClientsRepository)
        {

            _userManager = userManager;
            _profileRepository = profileRepository;
            _urlSettings = urlSettings;
            _fileService = fileService;
            _userOnlineSignalService = userOnlineSignalService;
            _notificationService = notificationService;
            _mailer = mailer;
            _oAuthClientsRepository = oAuthClientsRepository;
        }


        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Auth")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Auth(AuthModel userModel)
        {
            var dict=new Dictionary<string,string>();
            dict.Add("grant_type", "password");
            dict.Add("userName", userModel.Login);
            dict.Add("password", userModel.Password);
            using (var client = new HttpClient())
            {
               var response=await client.PostAsync($"{_urlSettings.ApiUrl}/api/token", new FormUrlEncodedContent(dict));
               var resultStr=await response.Content.ReadAsStringAsync();
               var tokenResp=JsonConvert.DeserializeObject<TokenDto>(resultStr);
                if (String.IsNullOrEmpty(tokenResp.Error) && String.IsNullOrEmpty(tokenResp.ErrorDescription))
                {
                    return SuccessApiResult(tokenResp);
                }
                else
                {
                    return ErrorApiResult(16, tokenResp.Error);
                }
                
            }
            
          
        }


        // POST api/Account/RegisterExternal
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            var user = await _userManager.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                return BadRequest("External user is already registered");
            }

            user = new ApplicationUser() { UserName = model.Email,ConfirmEmail = true};

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                var findedUser=await _userManager.FindByNameAsync(model.Email);
                if (findedUser == null)
                {
                    return GetErrorResult(result);
                }
                else
                {
                    user.Id = findedUser.Id;
                }
                
            }

            var info = new ExternalLoginInfo()
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
            };

            result = await _userManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName,user.Id,user.IsFilled);

            return Ok(accessTokenResponse);
        }

        private JObject GenerateLocalAccessTokenResponse(string email,long id,bool isFilled)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            var identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, email));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
            identity.AddClaim(new Claim("profileFiled", isFilled.ToString()));

            
            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = AuthSettings.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                new JProperty("userName", email),
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString()),
                    new JProperty("isFilled", isFilled.ToString()),
                        new JProperty("userId", id.ToString()));
        

            return tokenResponse;
        }

        // POST api/Account/Register
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Register")]
        [System.Web.Http.HttpPost]
     
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessages = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                return ErrorApiResult(1, errorsMessages);
            }


            var user = new ApplicationUser()
            {
                UserName = userModel.Email,
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded)
            {

                try
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                   
                    var callbackUrl = String.Format("{0}/loginpage?userId={1}&code={2}&email={3}", _urlSettings.SiteUrl,user.Id,
                        WebUtility.UrlEncode(code), user.UserName);
                    await _userManager.SendEmailAsync(user.Id, "ConfirmEmail", callbackUrl);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                return EmptyApiResult();
            }
            else
            {
                var errorsMessages = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                if (!errorsMessages.Any()) return ErrorApiResult(12, "User exist");
                return ErrorApiResult(1, errorsMessages);
            }

        }

        // POST api/Account/Register
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("search")]
        [System.Web.Http.HttpPost]

        public async Task<IHttpActionResult> Search(PatternModel pattern)
        {
           var result= await _profileRepository.Search(pattern.Pattern);
            return SuccessApiResult(result);
        }


        /// <summary>
        /// test
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.Authorize]
        [System.Web.Http.Route("checkactivity")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CheckActivity()
       {
            var userId = long.Parse(User.Identity.GetUserId());
            var result = await _profileRepository.CheckActivity(userId);
            return SuccessApiResult(result);
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = "235373570153403|jmPA2VY-SkNKl9n2q5SOM_M_I70";
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(AuthSettings.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == "Google")
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(AuthSettings.googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                }

            }

            return parsedToken;
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("VerifyEmail")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> VerifyEmail(VerifyEmailModel model)
        {
          
            if (model.UserId == null || model.Code == null)
            {
                return ErrorApiResult(1, "Нет параметров");
            }
            var userId = long.Parse(model.UserId);
            IdentityResult result = await _userManager.ConfirmEmailAsync(userId, model.Code);

            if (result.Succeeded)
            {
                return EmptyApiResult();
            }
            else
            {
                var errorsMessages = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                return ErrorApiResult(1, errorsMessages);
            }

            
        }


        [System.Web.Http.OverrideAuthentication]
        [System.Web.Http.HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ExternalLoginData externalLogin = await ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity,provider);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            if (provider == "Facebook")
            {
                
            }

            var user = await _userManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}&email={5}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.LoginProvider,
                                            hasRegistered.ToString(),
                                            externalLogin.UserName,
                                            externalLogin.Email);

            return Redirect(redirectUri);

        }
       

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {

            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                return BadRequest("Provider or external access token is not sent");
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            var user = await _userManager.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (!hasRegistered)
            {
                return BadRequest("External user is not registered");
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName,user.Id,user.IsFilled);

            return Ok(accessTokenResponse);

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return ErrorApiResult(1, "Bad request");
                }
                var errorsMessages = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

                return ErrorApiResult(100, errorsMessages);
            }

            return null;
        }



        [System.Web.Http.Authorize]
        [System.Web.Http.Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessages = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                return ErrorApiResult(1, errorsMessages);
            }
            var name = User.Identity.GetUserName();
            var userId = long.Parse(User.Identity.GetUserId());
            var result = await _userManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return EmptyApiResult();
            }
            else
            {
                return ErrorApiResult(2, result.Errors);
            }

        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("GetProfile")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetProfile(IdModel model)
        {
            long userId = 0;
            if (model != null && model.Id != null)
            {
                userId = (long) model.Id;
            }
            else
            {
                userId = long.Parse(User.Identity.GetUserId());
            }
           
            var profile = await _profileRepository.GetProfile(userId);
        
            if (profile == null)
            {
                return ErrorApiResult(12, "Profile not finded");
            }
            else
            {
                profile.IsOnline = _userOnlineSignalService.GetOnlineStatus(userId);
                return SuccessApiResult(profile);
            }
        }


        [System.Web.Http.Authorize]
        [System.Web.Http.Route("GetShortProfile")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetShortProfile(IdModel model)
        {
            long userId = 0;
            if (model != null && model.Id != null)
            {
                userId = (long) model.Id;
            }
            else
            {
                userId = long.Parse(User.Identity.GetUserId());
            }

            var profile = await _profileRepository.GetShortProfile(userId);
            if (profile == null)
            {
                return ErrorApiResult(12, "Profile not finded");
            }
            else
            {
                profile.IsOnline = _userOnlineSignalService.GetOnlineStatus(userId);
                return SuccessApiResult(profile);
            }
        }


        [System.Web.Http.Authorize]
        [System.Web.Http.Route("UpdateProfile")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> UpdateProfile(ProfileDto profileDto)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            profileDto.Id = userId;
            if (profileDto.Image != null)
            {
                profileDto.AvatarUrl=_fileService.SaveBase64FileReturnUrl(FileType.Image, profileDto.Image.Type, profileDto.Image.Result);
            }
            await _profileRepository.UpdateProfile(profileDto);
            dynamic jsonObject = new JObject();
            jsonObject.ProfileProgress = profileDto.ProfileProgress;
       
            return SuccessApiResult(jsonObject);
        }



        [System.Web.Http.Route("RecoverPassword")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> RecoverPassword(RecoverPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessages = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                return ErrorApiResult(1, errorsMessages);
            }
            var user = _userManager.FindByEmailAsync(model.Email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user.Result.Id);
           
            var callbackUrl = String.Format("{0}/recover?email={1}&token={2}",_urlSettings.SiteUrl, model.Email, WebUtility.UrlEncode(token));
            await _userManager.SendEmailAsync(user.Result.Id, "RecoverPassword", callbackUrl);
            return EmptyApiResult();
        }

        [System.Web.Http.Route("ResetPassword")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorsMessages = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                return ErrorApiResult(1, errorsMessages);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "No user found.");
                var errorsMessages = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                return ErrorApiResult(1, errorsMessages);
            }
            IdentityResult result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return EmptyApiResult();
            }
            else
            {
                var errorsMessages = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                return ErrorApiResult(1, errorsMessages);
            }

        }

        private  string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {

            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }

            var client =  _oAuthClientsRepository.FindClient(clientId);

            if (client == null)
            {
                return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            }

            if (client.AllowedOrigin != "*")
            {
                if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
                {
                    return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
                }
            }
           

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }


        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }
    }
}