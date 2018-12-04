using System;
using System.Collections.Generic;

namespace SimpleFacebookBackend.Models
{
    public partial class Friend
    {
        public int Id { get; set; }
        public int IdUser1 { get; set; }
        public int IdUser2 { get; set; }

        public User IdUser1Navigation { get; set; }
        public User IdUser2Navigation { get; set; }
    }
}
