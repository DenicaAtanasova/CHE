namespace CHE.Web.ViewModels.Teachers
{
    public class TeacherAllListViewModel
    {
        public PaginatedList<TeacherAllViewModel> Teachers { get; set; }

        public TeacherAllFilterViewModel Filter { get; set; }
    }
}
