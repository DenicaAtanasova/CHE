namespace CHE.Web.ViewModels.Cooperatives
{
    using System.Collections.Generic;

    public class CooperativeMembersViewModel
    {
        public string Id { get; set; }

        public string ScheduleId { get; set; }

        public string CreatorUserName { get; set; }

        public IEnumerable<CooperativeUserDetailsViewModel> Members { get; set; }
    }
}