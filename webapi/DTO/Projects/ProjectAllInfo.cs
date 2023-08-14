using webapi.Models;

namespace webapi.DTO.Projects
{
    public class ProjectAllInfo
    {
        public string? Name { get; set; }
        public DateTime TimeStamp { get; set; }

        public ProjectAllInfo() { }

        public ProjectAllInfo(Project project)
        {
            Name = project.Name;
            TimeStamp = project.TimeStamp;
        }
    }
}
