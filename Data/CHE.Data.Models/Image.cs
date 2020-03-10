namespace CHE.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Url { get; set; }

        public string Caption { get; set; }

        public string PortfolioId { get; set; }

        public Portfolio Portfolio { get; set; }
    }
}