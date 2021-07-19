namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;
    using System.Collections.Generic;

    public class Address : BaseModel<string>
    {
        public Address()
        {
            this.Cooperatives = new HashSet<Cooperative>();
        }

        public string City { get; set; }

        public string Neighbourhood { get; set; }

        public ICollection<Cooperative> Cooperatives { get; set; }
    }
}