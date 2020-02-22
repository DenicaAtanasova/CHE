namespace CHE.Services.Data
{
    using CHE.Data.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGradesService
    {
        Task<Grade> GetByValue(string value);

        Task<IEnumerable<string>> GetAllAsync();
    }
}