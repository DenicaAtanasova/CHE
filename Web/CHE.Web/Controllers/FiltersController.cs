namespace CHE.Web.Controllers
{
    using CHE.Data.Models;
    using CHE.Web.Cache;

    using Microsoft.AspNetCore.Mvc;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class FiltersController : ControllerBase
    {
        private readonly IAddressCache _addressCache;

        public FiltersController(IAddressCache addressCache)
        {
            _addressCache = addressCache;
        }

        [HttpGet("cities")]
        public async Task<IEnumerable<string>> Cities() =>
            await _addressCache.GetAsync(CacheType.City);


        [HttpGet("neighbourhoods")]
        public async Task<IEnumerable<string>> Neighbourhoods() =>
            await _addressCache.GetAsync(CacheType.Neighbourhood);

        [HttpGet("grades")]
        public IEnumerable<string> Grades() =>
            Enum.GetValues<Grade>()
                .Select(x => x.ToString());

        [HttpGet("schoolLevels")]
        public IEnumerable<string> SchoolLevels() =>
            Enum.GetValues<SchoolLevel>()
                .Where(x => x.ToString() != "Unknown")
                .Select(x => x.ToString());
    }
}