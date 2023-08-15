using System.ComponentModel.DataAnnotations;

namespace webapi.Requests.Project
{
    public class ProjectUpdateRequest
    {
        [EmailAddress]
        public string? UserEmail { get; set; }
        public string? Name { get; set; }
    }
}
