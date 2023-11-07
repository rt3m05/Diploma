namespace webapi.Models
{
    public class Tab
    {
        private User User { get; set; }
        private Project Project { get; set; }

        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public DateTime TimeStamp { get; set; }

        public Tab() { }
    }
}
