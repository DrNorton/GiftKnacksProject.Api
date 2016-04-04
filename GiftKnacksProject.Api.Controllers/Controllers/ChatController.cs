using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using GiftKnackProject.NotificationTypes.Chat;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.Chat;
using GiftKnacksProject.Api.Dto.Dtos.Gifts;
using GiftKnacksProject.Api.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/Chat")]
    public class ChatController : CustomApiController
    {
        private readonly IChatMessageService _chatMessageService;
        private readonly IProfileRepository _profileRepository;

        public ChatController(IChatMessageService chatMessageService,IProfileRepository profileRepository)
        {
            _chatMessageService = chatMessageService;
            _profileRepository = profileRepository;
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("send")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Send(SendToChatMessageDto newChatMessage)
        {
            var currentUser = long.Parse(User.Identity.GetUserId());
            await
                _chatMessageService.SendMessageToQueue(new ChatMqMessage()
                {
                    From = currentUser, 
                    Message = newChatMessage.Message,
                    To = newChatMessage.To
                });

            return SuccessApiResult(currentUser);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("getdialogs")]
        [System.Web.Http.HttpPost]

        public async Task<IHttpActionResult> GetDialogs()
        {
            var currentUser = long.Parse(User.Identity.GetUserId());
            var dialogList=await _chatMessageService.GetDialogs(currentUser);
            return SuccessApiResult(dialogList);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("getdialog")]
        [System.Web.Http.HttpPost]

        public async Task<IHttpActionResult> GetMessagesFromDialog(GetDialogDto getdialogs)
        {
            var currentUser = long.Parse(User.Identity.GetUserId());
            var dialogList = await _chatMessageService.GetMessagesFromDialog(currentUser, getdialogs.Recipient);
            if (dialogList.Any())
            {
                var profiles= await _profileRepository.GetTinyProfiles(new List<long>() {currentUser, getdialogs.Recipient});
                var members=new JObject();
                foreach (var profile in profiles)
                {
                    members.Add(profile.Id.ToString(),
                        new JObject()
                        {
                            new JProperty("FirstName", profile.FirstName),
                            new JProperty("LastName", profile.LastName),
                            new JProperty("AvatarUrl", profile.AvatarUrl)
                        });
                }
                return SuccessApiResult(new DialogWithMessagesDto() {Members = members, Messages = dialogList});

            }
            return SuccessApiResult(new DialogWithMessagesDto() { });

        }

    }
}
