namespace CHE.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class CheUser : IdentityUser
    {
        public CheUser()
        {
            this.Cooperatives = new HashSet<CheUserCooperative>();
        }

        public ICollection<CheUserCooperative> Cooperatives { get; set; }
    }
}