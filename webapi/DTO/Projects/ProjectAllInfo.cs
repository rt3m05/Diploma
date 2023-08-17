using webapi.Models;

namespace webapi.DTO.Projects
{
    public class ProjectAllInfo
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsMyProject { get; set; }

        public ProjectAllInfo() { }

        public ProjectAllInfo(Project project, bool isMyProject)
        {
            Id = project.Id;
            Name = project.Name;
            TimeStamp = project.TimeStamp;
            IsMyProject = isMyProject;
        }
    }
}
