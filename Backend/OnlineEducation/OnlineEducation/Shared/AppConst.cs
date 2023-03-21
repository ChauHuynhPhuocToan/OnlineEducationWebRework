namespace OnlineEducation.Shared
{
    public class AppConst
    {
        public const int BasicStringEmptyLength = 0;
        public const int BasicStringMinLength = 1;
        public const int BasicStringLength = 50;
        public const int BasicStringMediumLength = 100;
        public const int BasicStringLongLength = 255;
        public const int BasicLargeLongLength = 500;
        public static string LocalFileSavePath = "~/FileStorage";
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }

}
