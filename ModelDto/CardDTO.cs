namespace MTGProject.ModelDto;
    public class CardDTO
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public string Artist { get; init; }
        public string Image { get; init; }
        public string Color { get; init; }
        public string ManaCost { get; init; }
        public string ConvertedManaCost { get; init; }
        public string Type { get; init; }
        public string Text { get; init; }
        public string Flavor { get; init; }
        public string RarityCode { get; init; }
        public string SetCode { get; init; }
        public string Rarity { get; init; }
    }
    