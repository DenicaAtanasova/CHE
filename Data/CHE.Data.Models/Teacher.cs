namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Teacher : BaseModel<string>
    {
        public Teacher()
        {
            this.ReviewsReceived = new HashSet<Review>();
        }

        public Profile Profile { get; init; }

        public ICollection<Review> ReviewsReceived { get; set; }

        public Schedule Schedule { get; init; }

        [Required]
        public string UserId { get; set; }

        public CheUser User { get; set; }
    }
}