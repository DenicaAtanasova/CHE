namespace CHE.Web.ViewModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.Collections.Generic;

    public class CooperativeMembersViewModel : IMapFrom<Cooperative>
    {
        public string Id { get; init; }

        public string ScheduleId { get; init; }

        public string AdminUserName { get; init; }

        public IEnumerable<CooperativeUserDetailsViewModel> Members { get; set; }
    }
}