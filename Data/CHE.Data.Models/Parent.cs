namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Parent : BaseModel<string>
    {
        public Parent()
        {
            this.Cooperatives = new HashSet<ParentCooperative>();
            this.JoinRequestsSent = new HashSet<JoinRequest>();
            this.ReviewsSent = new HashSet<Review>();
        }

        public ICollection<ParentCooperative> Cooperatives { get; set; }

        public ICollection<JoinRequest> JoinRequestsSent { get; set; }

        public ICollection<Review> ReviewsSent { get; set; }

        [Required]
        public string UserId { get; set; }

        public CheUser User { get; set; }
    }
}