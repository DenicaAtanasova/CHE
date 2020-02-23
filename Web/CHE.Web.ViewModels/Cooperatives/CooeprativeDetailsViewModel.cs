namespace CHE.Web.ViewModels.Cooperatives
{
    public class CooeprativeDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Info { get; set; }

        public string Grade { get; set; }

        public int MembersCount { get; set; }

        public CooperativeAddressViewModel Address { get; set; }
    }
}