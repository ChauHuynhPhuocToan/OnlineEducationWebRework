namespace OnlineEducation.Models
{
    public class BaseRequestClasses
    {
        public string? FilterText { get; set; }
        public string? Sorting { get; set; } = string.Empty;
        public int MaxResultCount { get; set; } = 10;
        public int SkipCount { get; set; } = 0;
    }
}
