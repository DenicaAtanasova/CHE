namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.Address;

    public class Address : BaseModel<string>
    {
        public Address()
        {
            this.Cooperatives = new HashSet<Cooperative>();
        }

        [Required]
        [MaxLength(CityMaxLength)]
        public string City { get; set; }

        [Required]
        [MaxLength(NeighbourhoodMaxLength)]
        public string Neighbourhood { get; set; }

        public ICollection<Cooperative> Cooperatives { get; set; }
    }
}