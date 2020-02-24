namespace CHE.Data.Models
{
    using System.Collections.Generic;
    public class Parent : CheUser
    {
        public Parent()
        {
            this.ReviewsSent = new HashSet<Review>();
        }

        public ICollection<Review> ReviewsSent { get; set; }
    }
}