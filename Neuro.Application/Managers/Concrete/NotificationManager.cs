using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.IO;
using System.Threading.Tasks;
using Neuro.Application.Managers.Abstract;

namespace Neuro.Application.Managers.Concrete
{
    public class NotificationManager : INotificationManager
    {
        private readonly FirebaseApp _firebaseApp;

        public NotificationManager()
        {
            var applicationRoot = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Neuro.Application")).FullName;
            var configFilePath = Path.Combine(applicationRoot, "firebase-config-file.json");
            
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Firebase configuration file not found at the specified path.", configFilePath);
            }

            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(configFilePath),
            });
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