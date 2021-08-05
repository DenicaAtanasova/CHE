namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;

    using System.Collections.Generic;

    public  class Messenger : BaseModel<string>
    {
        public Messenger()
        {
            this.Users = new HashSet<MessengerUser>();
            this.Messages = new HashSet<Message>();
        }

        public string CooperativeId { get; set; }

        public Cooperative Cooperative { get; set; }

        public ICollection<MessengerUser> Users { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}