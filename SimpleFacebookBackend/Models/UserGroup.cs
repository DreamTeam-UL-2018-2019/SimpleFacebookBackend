using System;
using System.Collections.Generic;

namespace SimpleFacebookBackend.Models
{
    public partial class UserGroup
    {
        public int Id { get; set; }
        public int IdGroup { get; set; }
        public int IdUser { get; set; }

        public Group IdGroupNavigation { get; set; }
        public User IdUserNavigation { get; set; }
    }
}
