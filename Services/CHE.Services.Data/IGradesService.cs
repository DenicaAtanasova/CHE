namespace CHE.Services.Data
{
    using CHE.Data.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGradesService
    {
        Task<Grade> GetByValueAsync(string value);

        Task<IEnumerable<string>> GetAllValuesAsync(string currentGrade = null);
    }
}