using System;
using System.Collections.Generic;

namespace SimpleFacebookBackend.Models
{
    public partial class Message
    {
        public Message()
        {
            Unread = new HashSet<Unread>();
        }

        public int Id { get; set; }
        public int IdSender { get; set; }
        public int? IdReceiver { get; set; }
        public string Message1 { get; set; }
        public DateTime Date { get; set; }
        public string File { get; set; }
        public int? IdGroup { get; set; }

        public Group IdGroupNavigation { get; set; }
        public User IdReceiverNavigation { get; set; }
        public User IdSenderNavigation { get; set; }
        public ICollection<Unread> Unread { get; set; }
    }
}
