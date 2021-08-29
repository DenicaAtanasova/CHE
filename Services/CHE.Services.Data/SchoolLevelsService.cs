namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CHE.Data.Models.Enums;

    public class SchoolLevelsService : ISchoolLevelsService
    {
        public IEnumerable<string> GetAll() =>
            Enum.GetValues<SchoolLevel>()
                .Select(x => x.ToString());
    }
}