using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using webapi.Models;

namespace webapi.Requests.TileItem
{
    public class TileItemCreateRequest
    {
        [Required]
        public Guid TileId { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TileItemTypes Type { get; set; }
        [Required]
        public byte Position { get; set; }
        [Required]
        public bool IsDone { get; set; }
    }
}
