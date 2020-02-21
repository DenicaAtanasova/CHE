namespace CHE.Data.Models
{
    public class Address
    {
        public string Id { get; set; }

        public string City { get; set; }

        public string Neighbourhood { get; set; }

        public string Street { get; set; }

        public Cooperative Cooperative { get; set; }
    }
}