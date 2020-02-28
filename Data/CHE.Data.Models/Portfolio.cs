namespace CHE.Data.Models
{
    using Common.Models;
    using System.Collections.Generic;

    public class Portfolio : BaseDeletableModel<string>
    {
        public Portfolio()
        {
            this.Grades = new HashSet<PortfolioGrade>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Education { get; set; }

        public string Experience { get; set; }

        public string Skills { get; set; }

        public string Interests { get; set; }

        public string OwnerId { get; set; }

        public CheUser Owner { get; set; }

        public ICollection<PortfolioGrade> Grades { get; set; }
    }
}