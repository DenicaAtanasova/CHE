namespace CHE.Data.Models
{
    using Common.Models;
    using System.Collections.Generic;

    public class VCard : BaseDeletableModel<string>
    {
        public VCard()
        {
            this.Grades = new HashSet<VCardGrade>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Education { get; set; }

        public string Experience { get; set; }

        public string Skills { get; set; }

        public string Interests { get; set; }

        public Teacher Owner { get; set; }

        public ICollection<VCardGrade> Grades { get; set; }
    }
}