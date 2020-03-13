namespace CHE.Web.InputModels.Cooperatives
{
    using System.ComponentModel.DataAnnotations;

    public class CooperativeAddressInputModel
    {
        [Required]
        public string City { get; set; }

        public string Neighbourhood { get; set; }

        public string Street { get; set; }
    }
}