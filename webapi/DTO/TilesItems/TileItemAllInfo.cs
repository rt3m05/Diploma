using System.Text.Json.Serialization;
using webapi.Models;

namespace webapi.DTO.TilesItems
{
    public class TileItemAllInfo
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TileItemTypes Type { get; set; }
        public DateTime TimeStamp { get; set; }

        public TileItemAllInfo() { }

        public TileItemAllInfo(TileItem tileItem)
        {
            Id = tileItem.Id;
            Content = tileItem.Content;
            Type = tileItem.Type;
            TimeStamp = tileItem.TimeStamp;
        }
    }
}
