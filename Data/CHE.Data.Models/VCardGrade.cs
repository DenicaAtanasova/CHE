using System;
using System.Collections.Generic;
using System.Text;

namespace CHE.Data.Models
{
    public class VCardGrade
    {
        public string VCardId { get; set; }

        public VCard VCard { get; set; }

        public string GradeId { get; set; }

        public Grade Grade { get; set; }
    }
}