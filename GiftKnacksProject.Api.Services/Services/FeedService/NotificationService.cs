using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using GiftKnackProject.NotificationTypes;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using GiftKnacksProject.Api.Services.Interfaces;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;


namespace GiftKnacksProject.Api.Services.Services.FeedService
{
    public class NotificationService : INotificationService
    {
        private QueueClient _queueClient;
        private readonly DocumentClient _databaseClient;
        private const string DatabaseId = "knackgifterstorage";


        public NotificationService(QueueClient notififactionQueueClient, DocumentClient databaseClient)
        {
            _queueClient = notififactionQueueClient;
            _databaseClient = databaseClient;
        }

        public  Task SentNotificationToQueue(BaseQueueNotification queueNotification)
        {
            var message=new BrokeredMessage(queueNotification.GetJson());
            message.Properties["Type"] = queueNotification.Type;
            return _queueClient.SendAsync(message);
        }

        public async Task<List<Notification>> GetLenta(long id)
        {
            var database = await RetrieveOrCreateDatabaseAsync(DatabaseId);
            var collection = await RetrieveOrCreateCollectionAsync(database.SelfLink, "notificationLenta");
            var data = _databaseClient.CreateDocumentQuery<Notification>(collection.DocumentsLink).Where(x=>x.TargetUserId==id).OrderByDescending(x=>x.Time).ToList();
           

            return data;
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
