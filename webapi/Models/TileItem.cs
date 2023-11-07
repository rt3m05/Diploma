using System.Text.Json.Serialization;

namespace webapi.Models
{
    public class TileItem
    {
        private User User { get; set; }
        private TileItemTypes TileType { get; set; }
        private Tile Tile { get; set; }

        public Guid Id { get; set; }
        public Guid TileId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public byte[]? Image { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TileItemTypes Type { get; set; }
        public byte Position { get; set; }
        public bool IsDone { get; set; }
        public DateTime TimeStamp { get; set; }

        public TileItem() { }
    }
}
