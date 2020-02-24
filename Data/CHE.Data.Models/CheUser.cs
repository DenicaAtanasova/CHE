namespace CHE.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class CheUser : IdentityUser
    {
        public CheUser()
        {
            this.Cooperatives = new HashSet<CheUserCooperative>();
            this.JoinRequestsSent = new HashSet<JoinRequest>();
            this.JoinRequestsReceived = new HashSet<JoinRequest>();
        }

        public ICollection<CheUserCooperative> Cooperatives { get; set; }

        public ICollection<JoinRequest> JoinRequestsSent { get; set; }

        public ICollection<JoinRequest> JoinRequestsReceived { get; }
    }
}