using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnackProject.NotificationTypes.Chat;
using GiftKnacksProject.Api.Dto.Dtos.Chat;

namespace GiftKnacksProject.Api.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task SendMessageToQueue(ChatMqMessage mqMessage);
        Task<DialogsResultDto> GetDialogs(long userId);
        Task<List<MessageFromDialog>> GetMessagesFromDialog(long user1, long user2);
    }
}
