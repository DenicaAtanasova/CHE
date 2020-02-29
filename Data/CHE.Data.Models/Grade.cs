namespace CHE.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Grade
    {
        public Grade()
        {
            this.Cooperatives = new HashSet<Cooperative>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Value { get; set; }

        public int NumValue { get; set; }

        public ICollection<Cooperative> Cooperatives { get; set; }
    }
}