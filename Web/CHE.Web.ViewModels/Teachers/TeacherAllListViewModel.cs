namespace CHE.Web.ViewModels.Teachers
{
    public class TeacherAllListViewModel
    {
        public PaginatedList<TeacherAllViewModel> Teachers { get; set; }

        public FilterViewModel Filter { get; set; }
    }
}