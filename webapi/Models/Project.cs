namespace webapi.Models
{
    public class Project
    {
        private User User { get; set; }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public DateTime TimeStamp { get; set; }

        public Project() { }
    }
}
