namespace CHE.Data.Models
{
    public class PortfolioGrade
    {
        public string PortfolioId { get; set; }

        public Portfolio Portfolio { get; set; }

        public string GradeId { get; set; }

        public Grade Grade { get; set; }
    }
}