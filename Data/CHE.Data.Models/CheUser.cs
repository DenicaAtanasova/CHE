namespace CHE.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class CheUser : IdentityUser
    {
        public Teacher Teacher { get; set; }

        public Parent Parent { get; set; }
    }
}