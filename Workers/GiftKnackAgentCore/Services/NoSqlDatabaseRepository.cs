using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnackProject.NotificationTypes.Chat;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GiftKnackAgentCore.Services
{
    public interface INoSqlDatabaseRepository
    {
        Task SaveNotification(IEnumerable<Notification> notifications);
        Task SaveMessageToLastMessages(ChatMqMessage message);

        Task SaveMessageToAllMessages(ChatMqMessage message);
    }

    public class NoSqlDatabaseRepository : INoSqlDatabaseRepository
    {
        private readonly DocumentClient _client;
        private const string DatabaseId = "knackgifterstorage";


        public NoSqlDatabaseRepository(DocumentClient client)
        {
            _client = client;
        }

        public async Task SaveNotification(IEnumerable<Notification> notifications)
        {
            var database = await RetrieveOrCreateDatabaseAsync();
            var collection = await RetrieveOrCreateCollectionAsync(database.SelfLink, "notificationLenta");

            foreach (var notification in notifications)
            {
               string notifs = JsonConvert.SerializeObject(notification, Formatting.Indented,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        });

                    await _client.CreateDocumentAsync(collection.DocumentsLink, JObject.Parse(notifs));
            }
        }

        public async Task SaveMessageToLastMessages(ChatMqMessage message)
        {
            var database = await RetrieveOrCreateDatabaseAsync();
            var collection = await RetrieveOrCreateCollectionAsync(database.SelfLink, "lastmessages");
            var lastMessage = new LastMessageDocumentDbSchema()
            {
                LastMessage = message.Message,
                Recepient = message.To,
                Sender = message.From,
                Time=DateTime.Now,IsRead = true
            };

            try
            {
              
                await _client.UpsertDocumentAsync(collection.DocumentsLink, lastMessage);
            }
            catch (Exception e)
            {
                
            }
          

        }

        public async Task SaveMessageToAllMessages(ChatMqMessage messagefromMq)
        {
            var database = await RetrieveOrCreateDatabaseAsync();
            var collection = await RetrieveOrCreateCollectionAsync(database.SelfLink, "messages");
            var message = new MessageDbSchema()
            {
                Message = messagefromMq.Message,
                Recepient = messagefromMq.To,
                Sender = messagefromMq.From,
                Time = DateTime.Now,
                IsRead = true
                
            };

            await _client.UpsertDocumentAsync(collection.DocumentsLink, message);
        }


        private async Task<Database> RetrieveOrCreateDatabaseAsync()
        {
            // Try to retrieve the database (Microsoft.Azure.Documents.Database) whose Id is equal to databaseId            
            var database = _client.CreateDatabaseQuery().Where(db => db.Id == DatabaseId).AsEnumerable().FirstOrDefault();

            // If the previous call didn't return a Database, it is necessary to create it
            if (database == null)
            {
                database = await _client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                Console.WriteLine("Created Database: id - {0} and selfLink - {1}", database.Id, database.SelfLink);
            }

            return database;
        }

        private async Task<DocumentCollection> RetrieveOrCreateCollectionAsync(string databaseSelfLink, string name)
        {
            // Try to retrieve the collection (Microsoft.Azure.Documents.DocumentCollection) whose Id is equal to collectionId
            var collection = _client.CreateDocumentCollectionQuery(databaseSelfLink).Where(c => c.Id == name).ToArray().FirstOrDefault();

            // If the previous call didn't return a Collection, it is necessary to create it
            if (collection == null)
            {
                collection = await _client.CreateDocumentCollectionAsync(databaseSelfLink, new DocumentCollection { Id = name });
            }

            return collection;
        }
    }
}
