using webapi.Models;

namespace webapi.DTO.Tiles
{
    public class TileAllInfo
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public byte Position { get; set; }
        public DateTime TimeStamp { get; set; }

        public TileAllInfo() { }

        public TileAllInfo(Tile tile)
        {
            Id = tile.Id;
            Name = tile.Name;
            Position = tile.Position;
            TimeStamp = tile.TimeStamp;
        }
    }
}
