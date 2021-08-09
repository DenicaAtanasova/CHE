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

        public FiltersController(IAddressesService addressesService)
        {
            this._addressesService = addressesService;
        }

        [HttpGet("cities")]
        public async Task<IEnumerable<string>> Cities() =>
            await this._addressesService.GetAllCitiesAsync();

        [HttpGet("neighbourhoods")]
        public async Task<IEnumerable<string>> Neighbourhoods() =>
            await this._addressesService.GetAllNeighbourhoodsAsync();

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