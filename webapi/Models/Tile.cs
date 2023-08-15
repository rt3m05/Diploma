namespace webapi.Models
{
    public class Tile
    {
        public Guid Id { get; set; }
        public Guid TabId { get; set; }
        public string? Name { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public ushort H { get; set; }
        public ushort W { get; set; }
        public DateTime TimeStamp { get; set; }

        public Tile() { }
    }
}
