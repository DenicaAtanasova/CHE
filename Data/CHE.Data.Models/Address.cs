namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;

    public class Address : BaseModel<string>
    {
        public string City { get; set; }

        public string Neighbourhood { get; set; }

        //TODO: Remove Street
        public string Street { get; set; }

        public Cooperative Cooperative { get; set; }
    }
}