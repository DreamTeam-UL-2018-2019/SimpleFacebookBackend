using System;
using System.Collections.Generic;

namespace SimpleFacebookBackend.Models
{
    public partial class Group
    {
        public Group()
        {
            Message = new HashSet<Message>();
            UserGroup = new HashSet<UserGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public ICollection<Message> Message { get; set; }
        public ICollection<UserGroup> UserGroup { get; set; }
    }
}
