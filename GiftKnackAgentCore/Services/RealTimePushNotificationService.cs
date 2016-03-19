using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using Newtonsoft.Json;
using RestSharp;

namespace GiftKnackAgentCore.Services
{
    public class RealTimePushNotificationService : IRealTimePushNotificationService
    {
        private RestClient _restClient;

        public RealTimePushNotificationService()
        {
            _restClient=new RestClient("http://giftknackapi.azurewebsites.net/api/push/send");
        }

        public async Task SentRealTimeMessages(IEnumerable<Notification> notifications)
        {

            string notifs = JsonConvert.SerializeObject(notifications, Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

            Debug.WriteLine(notifs);
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", notifs, ParameterType.RequestBody);
            var response =  await _restClient.ExecuteTaskAsync(request);
           
        }
    }
}
