namespace webapi.Models
{
    public class Tile
    {
        private User User { get; set; }
        private Tab Tab { get; set; }

        public Guid Id { get; set; }
        public Guid TabId { get; set; }
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public byte Position { get; set; }
        public DateTime TimeStamp { get; set; }

        public Tile() { }
    }
}
