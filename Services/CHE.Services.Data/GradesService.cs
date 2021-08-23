namespace CHE.Services.Data
{
    using CHE.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GradesService : IGradesService
    {
        public IEnumerable<string> GetAll() =>
            Enum.GetValues<Grade>()
                .Select(x => x.ToString());
    }
}