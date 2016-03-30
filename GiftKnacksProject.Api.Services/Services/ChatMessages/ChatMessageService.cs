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
        private readonly string _databaseId;

        public ChatMessageService(QueueClient chatQueueClient, DocumentClient databaseClient,IProfileRepository profileRepository,string databaseName)
        {
            _queueClient = chatQueueClient;
            _databaseClient = databaseClient;
            _profileRepository = profileRepository;
            _databaseId = databaseName;
        }

        public Task SendMessageToQueue(ChatMqMessage mqMessage)
        {
            var message = new BrokeredMessage(JsonConvert.SerializeObject(mqMessage));
            return _queueClient.SendAsync(message);
        }

        public async Task<DialogsResultDto> GetDialogs(long userId)
          {
            var database = await RetrieveOrCreateDatabaseAsync(_databaseId);
            var collection = await RetrieveOrCreateCollectionAsync(database.SelfLink, "lastmessages");
            var data = _databaseClient.CreateDocumentQuery<LastMessageDocumentDbSchema>(collection.DocumentsLink).Where(x => x.Recepient == userId ||x.Sender==userId).OrderByDescending(x => x.Time).ToList();
            return await CreateDialogsList(data,userId);
           
        }

        public async Task<List<MessageFromDialog>> GetMessagesFromDialog(long user1, long user2)
        {
            var database = await RetrieveOrCreateDatabaseAsync(_databaseId);
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
                    Sender = message.Sender,IsRead = message.IsRead
                });
            }

            return resultMessages;
        }

        private async Task<DialogsResultDto> CreateDialogsList(IEnumerable<LastMessageDocumentDbSchema> messages,long ownerId)
        {
            var dialogs = new List<DialogItemInListDto>();
            var usersIds=new List<long>();
            usersIds.AddRange(messages.Select(x => x.Recepient));
            usersIds.AddRange(messages.Select(x => x.Sender));
            usersIds = usersIds.Distinct().ToList();
            var profiles= await _profileRepository.GetTinyProfiles(usersIds);
            foreach (var message in messages)
            {
                var resultDialog = new DialogItemInListDto()
                {
                    LastMessage =
                        new LastMessage() {Text = message.LastMessage, Time = message.Time, UserId = message.Sender},
                    IsRead = message.IsRead
                };
              
                if (message.Sender == ownerId)
                {
                    //я отправитель последнего
                    resultDialog.Opponent = profiles.FirstOrDefault(x => x.Id == message.Recepient);
                }
                else
                {
                    //не я отправлял последнее сообщение
                    resultDialog.Opponent = profiles.FirstOrDefault(x => x.Id == message.Sender);
                }
                dialogs.Add(resultDialog);
            }

            return new DialogsResultDto() {Owner = profiles.FirstOrDefault(x=>x.Id==ownerId),Dialogs = dialogs};

        }

        private async Task<Database> RetrieveOrCreateDatabaseAsync(string id)
        {
            // Try to retrieve the database (Microsoft.Azure.Documents.Database) whose Id is equal to databaseId            
            var database = _databaseClient.CreateDatabaseQuery().Where(db => db.Id == _databaseId).AsEnumerable().FirstOrDefault();

            // If the previous call didn't return a Database, it is necessary to create it
            if (database == null)
            {
                database = await _databaseClient.CreateDatabaseAsync(new Database { Id = _databaseId });
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
