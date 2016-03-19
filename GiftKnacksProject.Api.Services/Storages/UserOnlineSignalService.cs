using System.Collections.Generic;
using System.Linq;
using GiftKnacksProject.Api.Services.Interfaces;

namespace GiftKnacksProject.Api.Services.Storages
{
    public class UserOnlineSignalService : IUserOnlineSignalService
    {
        private object _synch;
        private List<UserConnectionSignalR> _usersOnline;
        

        public UserOnlineSignalService()
        {
            _synch = new object();
            _usersOnline =new List<UserConnectionSignalR>();
        }

        public void AddUserToOnline(long userId,string connectionId)
        {
            lock (_synch)
            {
                var findedUser = _usersOnline.FirstOrDefault(x => x.UserId == userId);
                if (findedUser == null)
                {

                    var newUserOnline = new UserConnectionSignalR(userId);
                    newUserOnline.AddConnection(connectionId);
                    _usersOnline.Add(newUserOnline);
                }
                else
                {
                    findedUser.AddConnection(connectionId);
                }
            }
           
        }

        public void RemoveUserFromOnline(long userId, string connectionId)
        {
            lock (_synch)
            {
                var findedUser = _usersOnline.FirstOrDefault(x => x.UserId == userId);
                if (findedUser != null)
                {
                    findedUser.RemoveConnection(connectionId);
                    if (findedUser.IsEmpty())
                    {
                        _usersOnline.Remove(findedUser);
                    }
                }
            }
        }

        public bool GetOnlineStatus(long userId)
        {
            var findedUser = _usersOnline.FirstOrDefault(x => x.UserId == userId);
            return findedUser != null;
        }

        public UserConnectionSignalR FindUser(long userId)
        {
            var findedUser = _usersOnline.FirstOrDefault(x => x.UserId == userId);
            return findedUser;
        }
    }
}
