namespace CHE.Web.Tests.Data
{
    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels;
    using CHE.Web.ViewModels.Cooperatives;

    using System.Collections.Generic;
    using System.Linq;

    using static WebConstants;

    public class Cooperatives
    {
        public static IEnumerable<CooperativeAllViewModel> AllCooperatives =>
            new List<CooperativeAllViewModel>
            {
                new CooperativeAllViewModel(),
                new CooperativeAllViewModel(),
                new CooperativeAllViewModel()
            };

        public static CooperativesAllListViewModel CooperativesList =>
            new CooperativesAllListViewModel
            {
                Cooperatives = PaginatedList<CooperativeAllViewModel>
                    .Create(AllCooperatives, AllCooperatives.Count(), 1, DefaultPageSize),
                Filter = CooperativesFilter
            };

        public static FilterViewModel CooperativesFilter =>
            new FilterViewModel
            {
                Level = "First",
                City = "Sofia",
                Neighbourhood = "Vitosha"
            };

        public static CooperativeDetailsViewModel DetailsCooperative =>
            new CooperativeDetailsViewModel
            {
                Id = "id"
            };

        public static CooperativeCreateInputModel CreateCooperative =>
            new CooperativeCreateInputModel
            {
                Name = "Name",
                Info = "Info",
                Grade = "First",
                Address = new CooperativeAddressInputModel
                {
                    City = "Sofia",
                    Neighbourhood = "Vitosha"
                }
            };

        public static CooperativeUpdateInputModel UpdateCooperative =>
            new CooperativeUpdateInputModel
            {
                Id = "id",
                Name = "Name",
                Info = "Info",
                Grade = "First",
                Address = new CooperativeAddressInputModel
                {
                    City = "Sofia",
                    Neighbourhood = "Vitosha"
                }
            };

        public static CooperativeMembersViewModel Members =>
            new CooperativeMembersViewModel {
                Members = new List<CooperativeUserDetailsViewModel> 
                { 
                    new CooperativeUserDetailsViewModel
                    {
                        CooperativeId = "id"
                    }
                }
            };
    }
}