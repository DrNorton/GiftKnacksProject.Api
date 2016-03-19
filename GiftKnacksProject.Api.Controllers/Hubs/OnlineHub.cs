using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace GiftKnacksProject.Api.Controllers.Hubs
{
    [HubName("onlinehub")]
    public class OnlineHub:Hub
    {
        private readonly IUserOnlineSignalService _userOnlineSignalService;
        private readonly IProfileRepository _profileRepository;

        public OnlineHub(IUserOnlineSignalService userOnlineSignalService,IProfileRepository profileRepository)
        {
            _userOnlineSignalService = userOnlineSignalService;
            _profileRepository = profileRepository;
        }


        public override Task OnConnected()
        {
          
            Groups.Add(Context.ConnectionId, "users");
            var clientId = GetClientId();
            if (clientId == -1)
            {
                return null;
            }
            _userOnlineSignalService.AddUserToOnline(clientId, Context.ConnectionId);
             SetUserLastLoginTime(clientId);
            Debug.WriteLine("Подключение Id-{0} Время {1}", clientId, DateTime.Now);
            var context = GlobalHost.ConnectionManager.GetHubContext<OnlineHub>();
         
            return null;
        }

        private void SetUserLastLoginTime(long clientId)
        {
            _profileRepository.UpdateLastLoginTime(clientId, DateTime.Now);
        }

        [Authorize]
        public Task<int> GetUserOnline()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<OnlineHub>();
            return Task.FromResult(4);
        } 

        public override Task OnReconnected()
        {
            //var clientId = GetClientId();
            //if (clientId == -1)
            //{
            //    return null;
            //}
            //_userOnlineStorage.AddUserToOnline(clientId, Context.ConnectionId);
            //Debug.WriteLine("Реконнект Id-{0} Время {1}",clientId,DateTime.Now);
            //SetUserLastLoginTime(clientId);
            //Groups.Add(Context.ConnectionId, "users");
            return null;
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Groups.Remove(Context.ConnectionId, "users");
            var clientId = GetClientId();
            if (clientId == -1)
            {
                return null;
            }
            _userOnlineSignalService.RemoveUserFromOnline(clientId, Context.ConnectionId);
            Debug.WriteLine("Дисконнект Id-{0} Время {1}", clientId, DateTime.Now);
            return null;
        }

        private long GetClientId()
        {
            if (Context.User==null) return -1;
           var strId= Context.User.Identity.GetUserId();
            return long.Parse(strId);
        }
    }
}
