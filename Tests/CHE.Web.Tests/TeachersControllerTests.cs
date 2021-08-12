namespace CHE.Web.Tests
{
    using CHE.Web.Controllers;
    using CHE.Web.ViewModels;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class TeachersControllerTests
    {
        [Fact]
        public void AllShouldReturnViewWithCorrectTeachers()
        {
            MyMvc
                .Pipeline()
                .ShouldMap("/Teachers/All")
                .To<TeachersController>(c => c.All(With.Any<FilterViewModel>(), 1))
                .Which()
                .ShouldReturn()
                .View();
        }
    }
}
