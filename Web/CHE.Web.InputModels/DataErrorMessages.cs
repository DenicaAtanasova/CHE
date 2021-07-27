namespace CHE.Web.InputModels
{
    public record DataErrorMessages
    {
        public const string StringLengthErroMessage = "{0} length must be between {2} and {1} symbols!";

        public const string RangeErroMessage = "{0} must be between {2} and {1}!";

        public const string GradeErroMessage = "Grade must be between first and eighth!";

        public const string SchoolLevelErrorMessage = "School level must be Primary, Secondary or Unknown!";

        public const string DateAfterErrorMessage = "Date must be after {0}!";

        public const string RequiredIfNotNullErrorMessage = "{0} is required when {1} is filled!";
    }
}