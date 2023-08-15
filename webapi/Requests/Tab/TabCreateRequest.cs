using System.ComponentModel.DataAnnotations;

namespace webapi.Requests.Tab
{
    public class TabCreateRequest
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
