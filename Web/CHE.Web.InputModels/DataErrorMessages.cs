namespace CHE.Web.InputModels
{
    public record DataErrorMessages
    {
        public const string StringLengthErroMessage = "{0} length must be between {2} and {1} symbols!";

        public const string RangeErroMessage = "{0} must be between {2} and {1}!";

        public const string GradeErroMessage = "Grade must be between first and eighth.";

        public const string SchoolLevelErrorMessage = "School level must be Primary, Secondary or Unknown.";
    }
}