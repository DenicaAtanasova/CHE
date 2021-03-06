﻿namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;

    public class Image : BaseModel<string>
    { 

        public string Url { get; set; }

        public string Caption { get; set; }

        public string ProfileId { get; set; }

        public Profile Profile { get; set; }
    }
}