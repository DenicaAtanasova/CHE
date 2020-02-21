namespace CHE.Data.Models
{
    using System.Collections.Generic;
    public class Parent : CheUser
    {
        public Parent()
        {
            this.SentReviews = new HashSet<Review>();
            this.SentJoinRequests = new HashSet<JoinRequest>();
        }

        public ICollection<Review> SentReviews { get; set; }

        public ICollection<JoinRequest> SentJoinRequests { get; set; }
    }
}