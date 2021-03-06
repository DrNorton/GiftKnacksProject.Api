﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnackAgentCore.Services;
using GiftKnackNotificationAgent.Services;
using GiftKnacksProject.Api.EfDao;
using Microsoft.Azure;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace GiftKnackNotificationAgent
{
    public class Functions
    {
        private readonly IMessageFromMqProcessor _messageFromMqProcessor;
        private readonly IRealTimePushNotificationService _realTimePushNotificationService;

        public Functions(IMessageFromMqProcessor messageFromMqProcessor, IRealTimePushNotificationService realTimePushNotificationService, EfContext context)
        {
            _messageFromMqProcessor = messageFromMqProcessor;
            _realTimePushNotificationService = realTimePushNotificationService;
        }


        public  async Task ProcessQueueMessage([ServiceBusTrigger("knackgiftnotifications")] BrokeredMessage message,
      TextWriter logger)
        {
            var processedMessages=await _messageFromMqProcessor.ProcessMessage(message);
            Console.WriteLine(JsonConvert.SerializeObject(processedMessages));
            await _realTimePushNotificationService.SentRealTimeMessages(processedMessages);
        }


        


    }
}
