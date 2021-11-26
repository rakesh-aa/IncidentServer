using IncidentServer.Models;

namespace IncidentServer.Services
{
    public interface INotificationService
    {
        Task<NotificationResponse> GetNotifications();
    }
}