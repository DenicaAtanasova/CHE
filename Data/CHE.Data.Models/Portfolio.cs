namespace CHE.Data.Models
{
    using Common.Models;

    public class Portfolio : BaseDeletableModel<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Education { get; set; }

        public string Experience { get; set; }

        public string Skills { get; set; }

        public string Interests { get; set; }

        public EducationLevel EducationLevel { get; set; }

        public string OwnerId { get; set; }

        public CheUser Owner { get; set; }
    }
}