namespace CHE.Services.Data
{
    using System.Collections.Generic;

    public interface IGradesService
    {
        IEnumerable<string> GetAll();
    }
}