using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnackMessageAgent.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace GiftKnackMessageAgent
{
    public class Functions
    {
        private readonly IChatMessageFromMqProcessor _chatMessageFromMqProcessor;

        public Functions(IChatMessageFromMqProcessor chatMessageFromMqProcessor)
        {
            _chatMessageFromMqProcessor = chatMessageFromMqProcessor;
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public async Task ProcessQueueMessage([ServiceBusTrigger("knackgiftmessages")] BrokeredMessage message,
     TextWriter logger)
        {
           await _chatMessageFromMqProcessor.ProcessMessage(message);
        }
    }
}
