using System.Text.Json.Serialization;
using webapi.Models;

namespace webapi.DTO.TilesItems
{
    public class TileItemAllInfo
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public byte[]? Image { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TileItemTypes Type { get; set; }
        public byte Position { get; set; }
        public bool IsDone { get; set; }
        public DateTime TimeStamp { get; set; }

        public TileItemAllInfo() { }

        public TileItemAllInfo(TileItem tileItem)
        {
            Id = tileItem.Id;
            Content = tileItem.Content;
            Image = tileItem.Image;
            Type = tileItem.Type;
            Position = tileItem.Position;
            IsDone = tileItem.IsDone;
            TimeStamp = tileItem.TimeStamp;
        }
    }
}
