namespace CHE.Data.Models
{
    using System.Collections.Generic;

    public class Teacher : CheUser
    {
        public Teacher()
        {
            this.ReceivedJoinRequests = new HashSet<JoinRequest>();
            this.ReceivedReviews = new HashSet<Review>();
        }

        public string VCardId { get; set; }

        public VCard VCard { get; set; }

        public ICollection<JoinRequest> ReceivedJoinRequests { get; }

        public ICollection<Review> ReceivedReviews { get; set; }
    }
}