using System.ComponentModel.DataAnnotations;

namespace webapi.Requests.Project
{
    public class ProjectCreateRequest
    {
        [Required]
        public string? Name { get; set; }
    }
}
