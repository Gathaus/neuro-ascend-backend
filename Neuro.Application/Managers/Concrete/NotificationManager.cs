using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Neuro.Application.Managers.Abstract;
using Newtonsoft.Json;

namespace Neuro.Application.Managers.Concrete
{
    public class NotificationManager : INotificationManager
    {
        private readonly FirebaseApp _firebaseApp;

        public NotificationManager(IConfiguration configuration)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var firebaseConfig = configuration.GetSection("FireBase").Get<FireBaseConfig>(); // FireBaseConfig nesnesine dönüştür

                _firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(firebaseConfig)) // JSON string'ine dönüştür
                });
            }
            else
            {
                _firebaseApp = FirebaseApp.DefaultInstance;
            }
        }

        public async Task SendNotificationAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine("Bildirim gönderildi: " + response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bildirim gönderilemedi: " + ex.Message);
            }
        }
        
        public async Task SendNotificationToTopicAsync(string topic, string title, string body)
        {
            var message = new Message()
            {
                Topic = topic,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine("Bildirim gönderildi: " + response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bildirim gönderilemedi: " + ex.Message);
            }
        }

    }
}