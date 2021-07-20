namespace CHE.Data.Models
{
    using Common.Models;

    public class Profile : BaseModel<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Education { get; set; }

        public string Experience { get; set; }

        public string Skills { get; set; }

        public string Interests { get; set; }

        public SchoolLevel SchoolLevel { get; set; }

        public string OwnerId { get; set; }

        public CheUser Owner { get; set; }

        public Image Image { get; set; }

        public string AddressId { get; set; }

        public Address Address { get; set; }
    }
}