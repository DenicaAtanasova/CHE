namespace CHE.Web.ViewModels
{
    using System.Collections.Generic;

    public class FilterViewModel
    {
        public string LevelDisplayName { get; set; }

        public string Level { get; set; }

        public string City { get; set; }

        public string Neighbourhood { get; set; }

        public IEnumerable<string> Levels { get; set; }

        public IEnumerable<string> Cities { get; set; }

        public IEnumerable<string> Neighbourhoods { get; set; }
    }
}