namespace webapi.Requests.Project
{
    public class ProjectUpdateRequest
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
    }
}
