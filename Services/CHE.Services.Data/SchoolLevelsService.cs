namespace CHE.Services.Data
{
    using CHE.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SchoolLevelsService : ISchoolLevelsService
    {
        public IEnumerable<string> GetAll() =>
            Enum.GetValues<SchoolLevel>()
                .Where(x => x.ToString() != "Unknown")
                .Select(x => x.ToString());
    }
}