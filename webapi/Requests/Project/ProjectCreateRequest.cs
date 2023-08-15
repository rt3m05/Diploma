using System.ComponentModel.DataAnnotations;

namespace webapi.Requests.Project
{
    public class ProjectCreateRequest
    {
        [Required]
        [EmailAddress]
        public string? UserEmail { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
