namespace CHE.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Grade
    {
        public Grade()
        {
            this.Cooperatives = new HashSet<Cooperative>();
            this.vCards = new HashSet<VCardGrade>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Value { get; set; }

        public int NumValue { get; set; }

        public ICollection<Cooperative> Cooperatives { get; set; }

        public ICollection<VCardGrade> vCards { get; set; }
    }
}