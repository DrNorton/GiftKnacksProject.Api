using GiftKnacksProject.Api.Services.Storages;

namespace GiftKnacksProject.Api.Services.Interfaces
{
    public interface IUserOnlineSignalService
    {
        void AddUserToOnline(long userId, string connectionId);
        void RemoveUserFromOnline(long userId, string connectionId);
        bool GetOnlineStatus(long id);
        UserConnectionSignalR FindUser(long userId);
    }
}