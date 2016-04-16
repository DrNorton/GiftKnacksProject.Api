using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using GiftKnackAgentCore.Services;
using GiftKnackMessageAgent.Services;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos;
using GiftKnacksProject.Api.Dao.Repositories;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace GiftKnackMessageAgent
{
    public class Functions
    {
        private readonly IChatMessageFromMqProcessor _chatMessageFromMqProcessor;
        private readonly IRealTimePushNotificationService _realTimePushNotificationService;
        private readonly IProfileRepository _profileRepository;

        public Functions(IChatMessageFromMqProcessor chatMessageFromMqProcessor,IRealTimePushNotificationService realTimePushNotificationService,IProfileRepository profileRepository)
        {
            _chatMessageFromMqProcessor = chatMessageFromMqProcessor;
            _realTimePushNotificationService = realTimePushNotificationService;
            _profileRepository = profileRepository;
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public async Task ProcessQueueMessage([ServiceBusTrigger("knackgiftmessages")] BrokeredMessage message,
     TextWriter logger)
        {
           var messageAfterProcessing=await _chatMessageFromMqProcessor.ProcessMessage(message);
            var fromUser=await _profileRepository.GetTinyProfile(messageAfterProcessing.From);
            var newMessageNotification = new Notification()
            {
                Info =
                    new NewMessageInfo()
                    {
                        OwnerId = messageAfterProcessing.To,
                        From = fromUser,
                        Message = messageAfterProcessing.Message
                    },
                Action = "newMessage",
                Id = Guid.NewGuid().ToString(),
                Time = DateTime.Now
            };
            await _realTimePushNotificationService.SentRealTimeMessages(new List<Notification>() {newMessageNotification});
        }
    }
}
