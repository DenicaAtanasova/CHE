namespace CHE.Data.Models
{
    using Common.Models;
    using System.Collections.Generic;

    public class Grade
    {
        public Grade()
        {
            this.Cooperatives = new HashSet<Cooperative>();
            this.vCards = new HashSet<VCardGrade>();
        }

        public string Id { get; set; }

        public string Value { get; set; }

        public ICollection<Cooperative> Cooperatives { get; set; }

        public ICollection<VCardGrade> vCards { get; set; }
    }
}