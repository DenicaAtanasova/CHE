namespace CHE.Services.Data
{
    using System.Collections.Generic;

    public interface ISchoolLevelsService
    {
        IEnumerable<string> GetAll();
    }
}