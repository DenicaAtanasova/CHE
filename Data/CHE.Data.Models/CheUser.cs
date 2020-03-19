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
            this.ReviewsSent = new HashSet<Review>();
            this.ReviewsReceived = new HashSet<Review>();
        }

        public string RoleName { get; set; }

        public Portfolio Portfolio { get; set; }

        public ICollection<CheUserCooperative> Cooperatives { get; set; }

        public ICollection<JoinRequest> JoinRequestsSent { get; set; }

        public ICollection<JoinRequest> JoinRequestsReceived { get; }

        public ICollection<Review> ReviewsSent { get; set; }

        public ICollection<Review> ReviewsReceived { get; set; }

        public Schedule Schedule { get; set; }
    }
}