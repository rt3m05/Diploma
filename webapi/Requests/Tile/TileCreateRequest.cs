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
        public ushort X { get; set; }
        [Required]
        public ushort Y { get; set; }
        [Required]
        public ushort H { get; set; }
        [Required]
        public ushort W { get; set; }
    }
}
