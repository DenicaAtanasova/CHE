namespace CHE.Web.Tests.Data
{
    using CHE.Web.ViewModels.Schedules;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Schedules
    {
        public static ScheduleViewModel DetailsSchedule =>
            new ScheduleViewModel
            {
                CooperativeId = "cooperativeId"
            };
    }
}
