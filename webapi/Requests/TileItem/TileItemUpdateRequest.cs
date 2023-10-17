using System.Text.Json.Serialization;
using webapi.Models;

namespace webapi.Requests.TileItem
{
    public class TileItemUpdateRequest
    {
        public string? Content { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TileItemTypes? Type { get; set; }
        public byte? Position { get; set; }
        public bool? IsDone { get; set; }
    }
}
