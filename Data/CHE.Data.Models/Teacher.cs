namespace CHE.Data.Models
{
    using System.Collections.Generic;

    public class Teacher : CheUser
    {
        public Teacher()
        {
            this.ReviewsReceived = new HashSet<Review>();
        }

        public string VCardId { get; set; }

        public VCard VCard { get; set; }

        public ICollection<Review> ReviewsReceived { get; set; }
    }
}