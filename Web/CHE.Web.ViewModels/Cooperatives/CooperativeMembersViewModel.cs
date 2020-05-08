namespace CHE.Web.ViewModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.Collections.Generic;

    public class CooperativeMembersViewModel : IMapFrom<Cooperative>
    {
        public string Id { get; set; }

        public string ScheduleId { get; set; }

        public string CreatorUserName { get; set; }

        public IEnumerable<CooperativeUserDetailsViewModel> Members { get; set; }
    }
}