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


        public AccountController(CustomUserManager userManager, IProfileRepository profileRepository, UrlSettings urlSettings, IFileService fileService, IUserOnlineSignalService userOnlineSignalService, INotificationService notificationService,IUserMailer mailer)
        {

            _userManager = userManager;
            _profileRepository = profileRepository;
            _urlSettings = urlSettings;
            _fileService = fileService;
            _userOnlineSignalService = userOnlineSignalService;
            _notificationService = notificationService;
            _mailer = mailer;
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
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("checkactivity")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CheckActivity()
       {
            var name = User.Identity.GetUserName();
            if (String.IsNullOrEmpty(User.Identity.GetUserId()))
            {
                return ErrorApiResult(401, "Unauthorizate");
            }
            var userId = long.Parse(User.Identity.GetUserId());
            var result = await _profileRepository.CheckActivity(userId);
            return SuccessApiResult(result);
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

     


    }
}