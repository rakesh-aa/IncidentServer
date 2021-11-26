using System;
using IncidentServer.Models;
using Microsoft.SharePoint.Client;
using PnP.Framework;

namespace IncidentServer.Services
{
    public class NotificationService : INotificationService
    {
    private readonly IConfiguration _configuration;

        public NotificationService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public async Task<NotificationResponse> GetNotifications()
        {
            var notificationResponse = new NotificationResponse();

            var siteURL = _configuration["NotificationService:SiteURL"];
            var clientID = _configuration["NotificationService:ClientID"];
            var clientSecret = _configuration["NotificationService:ClientSecret"];

            using (var context = new AuthenticationManager().GetACSAppOnlyContext(siteURL, clientID, clientSecret))
            {
                context.Load(context.Web, p => p.Title);
                context.ExecuteQuery();

                var knownIssueList = context.Web.Lists.GetByTitle("Known Issues");

                var query = new CamlQuery();
                var items = knownIssueList.GetItems(query);

                context.Load(items);
                context.ExecuteQuery();

                var notifications = new List<Notification>();
                foreach (var item in items)
                {
                    var notification = new Notification();
                    notification.Title = item["Title"] as string;

                    if (item["DateofIssue_x002f_Change"] != null)
                    {
                        notification.IncidentDate = (DateTime)item["DateofIssue_x002f_Change"];
                    }
                    notification.Message = item["Description"] as string;
                    notification.GUID = Guid.NewGuid().ToString();
                    notification.HSPRTaskIncident = item["HSPR_x002f_Task_x002f_Incident"] as string;
                    notification.Resolution = item["Resolution"] as string;
                    notification.Location = item["Location_x0028_s_x0029_"] as string;
                    notification.DateResolved = item["DateResolved"] as string;

                    notifications.Add(notification);
                }

                notificationResponse.Notifications = notifications;
                return notificationResponse;
            }
        }
    }
}