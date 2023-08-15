using webapi.Models;

namespace webapi.DTO.Tiles
{
    public class TileAllInfo
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public ushort H { get; set; }
        public ushort W { get; set; }
        public DateTime TimeStamp { get; set; }

        public TileAllInfo() { }

        public TileAllInfo(Tile tile)
        {
            Id = tile.Id;
            Name = tile.Name;
            X = tile.X;
            Y = tile.Y;
            H = tile.H;
            W = tile.W;
            TimeStamp = tile.TimeStamp;
        }
    }
}
