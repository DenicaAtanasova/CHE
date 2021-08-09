namespace CHE.Web.Controllers
{
    using CHE.Data.Models;
    using CHE.Services.Data;

    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class FiltersController : ControllerBase
    {
        private readonly IAddressesService _addressesService;
        private readonly IGradesService _gradesService;

        public FiltersController(IAddressesService addressesService, IGradesService gradesService)
        {
            this._addressesService = addressesService;
            this._gradesService = gradesService;
        }

        [HttpGet("cities")]
        public async Task<IEnumerable<string>> Cities() =>
            await this._addressesService.GetAllCitiesAsync();

        [HttpGet("neighbourhoods")]
        public async Task<IEnumerable<string>> Neighbourhoods() =>
            await this._addressesService.GetAllNeighbourhoodsAsync();

        [HttpGet("grades")]
        public async Task<IEnumerable<string>> Grades() =>
            await this._gradesService.GetAllAsync();

        [HttpGet("schoolLevels")]
        public IEnumerable<string> SchoolLevels() =>
            Enum.GetValues(typeof(SchoolLevel))
                .Cast<SchoolLevel>()
                .Where(x => x.ToString() != "Unknown")
                .Select(x => x.ToString());
    }
}
