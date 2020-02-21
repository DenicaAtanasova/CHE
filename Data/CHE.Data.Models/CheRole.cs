namespace CHE.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class CheRole : IdentityRole
    {
        public CheRole()
            :this(null)
        {
        }

        public CheRole(string name)
            :base(name)
        {
        }
    }
}