using System.Text.Json.Serialization;

namespace webapi.Models
{
    public class TileItem
    {
        public Guid Id { get; set; }
        public Guid TileId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TileItemTypes Type { get; set; }
        public DateTime TimeStamp { get; set; }

        public TileItem() { }
    }
}
