namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Cache;

    using Microsoft.AspNetCore.Mvc;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class FiltersController : ControllerBase
    {
        private readonly IAddressCache _addressCache;
        private readonly IGradesService _gradesService;
        private readonly ISchoolLevelsService _schoolLevelsService;

        public FiltersController(
            IAddressCache addressCache,
            IGradesService gradesService,
            ISchoolLevelsService schoolLevelsService)
        {
            this._addressCache = addressCache;
            _gradesService = gradesService;
            _schoolLevelsService = schoolLevelsService;
        }

        [HttpGet("cities")]
        public async Task<IEnumerable<string>> Cities() =>
            await this._addressCache.GetAsync(CacheType.City);


        [HttpGet("neighbourhoods")]
        public async Task<IEnumerable<string>> Neighbourhoods() =>
            await this._addressCache.GetAsync(CacheType.Neighbourhood);

        [HttpGet("grades")]
        public IEnumerable<string> Grades() =>
            this._gradesService.GetAll();

        [HttpGet("schoolLevels")]
        public IEnumerable<string> SchoolLevels() =>
            this._schoolLevelsService.GetAll();            
    }
}