using System;

namespace IncidentServer.Models {

    public class NotificationResponse 
    {
        public List<Notification>? Notifications {get; set;}
    }

    public class Notification {

        public string? Title {get; set;}
        public string? Message {get; set;}
        public string? GUID {get; set;}
        public DateTime IncidentDate {get; set;}
        public string? HSPRTaskIncident {get; set;}
        public string? Resolution {get; set;}
        public string? Location {get; set;}
        public string? DateResolved {get; set;}
    }
}

