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
        public IFormFile? File { get; set; }

        public bool isEmpty()
        {
            return Content == null && Type == null && Position == null && IsDone == null && File == null;
        }

        public bool isOnlyPosition()
        {
            return Content == null && Type == null && IsDone == null && File == null && Position != null;
        }
    }
}
