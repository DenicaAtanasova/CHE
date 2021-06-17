namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGradesService
    {
        Task<string> GetGardeIdAsync(string value);

        Task<IEnumerable<string>> GetAllValuesAsync(string currentGrade = null);
    }
}