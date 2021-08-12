namespace CHE.Web.Tests.Data
{
    using CHE.Web.ViewModels;
    using CHE.Web.ViewModels.Teachers;

    using System.Collections.Generic;
    using System.Linq;

    using static WebConstants;

    public class Teachers
    {
        public static IEnumerable<TeacherAllViewModel> AllTeachers =>
            new List<TeacherAllViewModel>
            {
                new TeacherAllViewModel(),
                new TeacherAllViewModel(),
                new TeacherAllViewModel()
            };

        public static TeacherAllListViewModel TeachersList =>
            new TeacherAllListViewModel
            {
                Teachers = PaginatedList<TeacherAllViewModel>
                    .Create(AllTeachers, AllTeachers.Count(), 1, DefaultPageSize),
                Filter = new FilterViewModel()
            };

        public static TeacherDetailsViewModel DetailsTeacher =>
            new TeacherDetailsViewModel();
    }
}