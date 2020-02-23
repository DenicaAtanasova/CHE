namespace CHE.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string City { get; set; }

        public string Neighbourhood { get; set; }

        public string Street { get; set; }

        public Cooperative Cooperative { get; set; }
    }
}