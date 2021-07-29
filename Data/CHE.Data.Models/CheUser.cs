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
        //Teacher only
        public Profile Profile { get; init; }

        public ICollection<CheUserCooperative> Cooperatives { get; set; }
        //Parent only
        public ICollection<JoinRequest> JoinRequestsSent { get; set; }
        //Teacher only
        public ICollection<JoinRequest> JoinRequestsReceived { get; }
        //Parent only
        public ICollection<Review> ReviewsSent { get; set; }
        //Teacher only
        public ICollection<Review> ReviewsReceived { get; set; }
        //Teacher only
        public Schedule Schedule { get; init; }
    }
}