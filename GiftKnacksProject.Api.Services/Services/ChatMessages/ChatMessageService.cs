using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnackProject.NotificationTypes.Chat;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.Chat;
using GiftKnacksProject.Api.Services.Interfaces;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace GiftKnacksProject.Api.Services.Services.ChatMessages
{
    public class ChatMessageService: IChatMessageService
    {
        private readonly QueueClient _queueClient;
        private readonly DocumentClient _databaseClient;
        private readonly IProfileRepository _profileRepository;
        private const string DatabaseId = "knackgifterstorage";

        public ChatMessageService(QueueClient chatQueueClient, DocumentClient databaseClient,IProfileRepository profileRepository)
        {
            _queueClient = chatQueueClient;
            _databaseClient = databaseClient;
            _profileRepository = profileRepository;
        }

        public Task SendMessageToQueue(ChatMqMessage mqMessage)
        {
            var message = new BrokeredMessage(JsonConvert.SerializeObject(mqMessage));
            return _queueClient.SendAsync(message);
        }

        public async Task<List<DialogDto>> GetDialogs(long userId)
          {
            var database = await RetrieveOrCreateDatabaseAsync(DatabaseId);
            var collection = await RetrieveOrCreateCollectionAsync(database.SelfLink, "lastmessages");
            var data = _databaseClient.CreateDocumentQuery<LastMessageDocumentDbSchema>(collection.DocumentsLink).Where(x => x.Recepient == userId ||x.Sender==userId).OrderByDescending(x => x.Time).ToList();
            return await CreateDialogsList(data);
           
        }

        public async Task<List<MessageFromDialog>> GetMessagesFromDialog(long user1, long user2)
        {
            var database = await RetrieveOrCreateDatabaseAsync(DatabaseId);
            var resultMessages=new List<MessageFromDialog>();
            var collection = await RetrieveOrCreateCollectionAsync(database.SelfLink, "messages");
            string id;
            if (user1 > user2)
            {
                id = String.Format("{0}:{1}", user1, user2);
            }
            else
            {
                id = String.Format("{0}:{1}", user2, user1);
            }
            var messages =
                _databaseClient.CreateDocumentQuery<MessageDbSchema>(collection.DocumentsLink)
                    .Where(x => x.DialogId == id)
                    .OrderByDescending(x => x.Time)
                    .ToList();

            foreach (var message in messages)
            {
                resultMessages.Add(new MessageFromDialog()
                {
                    Message = message.Message,
                    Time = message.Time,
                    Recepient = message.Recepient,
                    Sender = message.Sender
                });
            }

            return resultMessages;
        }

        private async Task<List<DialogDto>> CreateDialogsList(IEnumerable<LastMessageDocumentDbSchema> messages)
        {
            var dialogs = new List<DialogDto>();
            var usersIds=new List<long>();
            usersIds.AddRange(messages.Select(x => x.Recepient));
            usersIds.AddRange(messages.Select(x => x.Sender));
            var profiles= await _profileRepository.GetTinyProfiles(usersIds);
            foreach (var message in messages)
            {
                dialogs.Add(new DialogDto() {LastMessage = message.LastMessage,Recipient = profiles.FirstOrDefault(x=>x.Id==message.Recepient),Sender = profiles.FirstOrDefault(x=>x.Id==message.Sender),Time = message.Time});
            }

            return dialogs;

        }

        private async Task<Database> RetrieveOrCreateDatabaseAsync(string id)
        {
            // Try to retrieve the database (Microsoft.Azure.Documents.Database) whose Id is equal to databaseId            
            var database = _databaseClient.CreateDatabaseQuery().Where(db => db.Id == DatabaseId).AsEnumerable().FirstOrDefault();

            // If the previous call didn't return a Database, it is necessary to create it
            if (database == null)
            {
                database = await _databaseClient.CreateDatabaseAsync(new Database { Id = DatabaseId });
                Console.WriteLine("Created Database: id - {0} and selfLink - {1}", database.Id, database.SelfLink);
            }

            return database;
        }

        private async Task<DocumentCollection> RetrieveOrCreateCollectionAsync(string databaseSelfLink, string id)
        {
            // Try to retrieve the collection (Microsoft.Azure.Documents.DocumentCollection) whose Id is equal to collectionId
            var collection = _databaseClient.CreateDocumentCollectionQuery(databaseSelfLink).Where(c => c.Id == id).ToArray().FirstOrDefault();

            // If the previous call didn't return a Collection, it is necessary to create it
            if (collection == null)
            {
                collection = await _databaseClient.CreateDocumentCollectionAsync(databaseSelfLink, new DocumentCollection { Id = id });
            }

            return collection;
        }
    }
}
