namespace MTGProject.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string UserId { get; set; }  
        public long CardId { get; set; }

        public Card Card { get; set; } = null!;
    }
}