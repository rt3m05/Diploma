using System.ComponentModel.DataAnnotations;

namespace webapi.Requests.Tile
{
    public class TileCreateRequest
    {
        [Required]
        public Guid TabId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public byte Position { get; set; }
    }
}
