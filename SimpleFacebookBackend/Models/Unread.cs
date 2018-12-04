using System;
using System.Collections.Generic;

namespace SimpleFacebookBackend.Models
{
    public partial class Unread
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdMessage { get; set; }

        public Message IdMessageNavigation { get; set; }
        public User IdUserNavigation { get; set; }
    }
}
