namespace CHE.Data.Seedeing
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GradesSeeder : ISeeder
    {
        private readonly IReadOnlyCollection<string> GRADES = 
            new string[] {"FIRST","SECOND", "THIRD", "FORTH", "FIFTH", "SIXTH", "SEVENTH", "EIGHTH" };

        public async Task SeedAsync(CheDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Grades.Any())
            {
                return;
            }

            var gradesForDb = new List<Grade>();
            foreach (var currentGrade in this.GRADES)
            {
                var grade = new Grade { Value = currentGrade };

                gradesForDb.Add(grade);
            }

            await dbContext.Grades.AddRangeAsync(gradesForDb);
        }
    }
}