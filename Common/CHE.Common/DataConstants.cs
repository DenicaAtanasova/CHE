namespace CHE.Common
{
    public record DataConstants
    {
        public record Address
        {
            public const int CityMinLength = 3;
            public const int CityMaxLength = 20;

            public const int NeighbourhoodMinLength = 3;
            public const int NeighbourhoodMaxLength = 20;
        }

        public record Cooperative
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;

            public const int InfoMinLength = 3;
            public const int InfoMaxLength = 1000;
        }

        public record JoinRequst
        {
            public const int ContentMinLength = 3;
            public const int ContentMaxLength = 30;
        }

        public record Review
        {
            public const int CommentMinLength = 3;
            public const int CommentMaxLength = 30;

            public const int RatingMinValue = 1;
            public const int RatingMaxValue = 5;
        }

        public record Profile
        {
            public const int FirstNameMinLength = 3;
            public const int FirstNameMaxLength = 15;

            public const int LastNameMinLength = 3;
            public const int LastNameMaxLength = 30;

            public const int EducationMaxLength = 200;

            public const int ExperienceMaxLength = 500;

            public const int SkillsMaxLength = 300;

            public const int InterestsMaxLength = 500;
        }

        public record Event
        {
            public const int TitleMaxLength = 30;

            public const int DescriptionMaxLength = 200;
        }
    }
}