using System;
using System.Collections.Generic;

namespace SimpleFacebookBackend.Models
{
    public partial class User
    {
        public User()
        {
            FriendIdUser1Navigation = new HashSet<Friend>();
            FriendIdUser2Navigation = new HashSet<Friend>();
            MessageIdReceiverNavigation = new HashSet<Message>();
            MessageIdSenderNavigation = new HashSet<Message>();
            Unread = new HashSet<Unread>();
            UserGroup = new HashSet<UserGroup>();
        }

        public int Id { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }

        public ICollection<Friend> FriendIdUser1Navigation { get; set; }
        public ICollection<Friend> FriendIdUser2Navigation { get; set; }
        public ICollection<Message> MessageIdReceiverNavigation { get; set; }
        public ICollection<Message> MessageIdSenderNavigation { get; set; }
        public ICollection<Unread> Unread { get; set; }
        public ICollection<UserGroup> UserGroup { get; set; }
    }
}
