namespace MTGProject.Models
{
    public class FilterData
    {
        public string SearchTerm { get; set; } = string.Empty;
        public string ManaCostFilter { get; set; } = string.Empty;
        public string TypeFilter { get; set; } = string.Empty;
        public string RarityFilter { get; set; } = string.Empty;
        public string ColorFilter { get; set; } = string.Empty;

        public bool HasAnyFilter =>
            !string.IsNullOrWhiteSpace(SearchTerm) ||
            !string.IsNullOrWhiteSpace(ManaCostFilter) ||
            !string.IsNullOrWhiteSpace(TypeFilter) ||
            !string.IsNullOrWhiteSpace(RarityFilter) ||
            !string.IsNullOrWhiteSpace(ColorFilter);
    }
}
