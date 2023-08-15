using System.Text.Json.Serialization;
using webapi.Models;

namespace webapi.Requests.TileItem
{
    public class TileItemUpdateRequest
    {
        public Guid? TileId { get; set; }
        public string? Content { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TileItemTypes? Type { get; set; }
    }
}
