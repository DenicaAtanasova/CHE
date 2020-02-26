namespace CHE.Data.Seedeing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

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
            var gradeNumValue = 1;
            foreach (var currentGrade in this.GRADES)
            {
                var grade = new Grade 
                { 
                    Value = currentGrade, 
                    NumValue = gradeNumValue
                };

                gradesForDb.Add(grade);
                gradeNumValue++;
            }

            await dbContext.Grades.AddRangeAsync(gradesForDb);
        }
    }
}