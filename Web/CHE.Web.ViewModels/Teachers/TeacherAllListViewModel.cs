namespace CHE.Web.ViewModels.Teachers
{
    using System.Collections.Generic;

    public class TeacherAllListViewModel
    {
        public PaginatedList<TeacherAllViewModel> Teachers { get; set; }

        public FilterViewModel Filter { get; set; }
    }
}