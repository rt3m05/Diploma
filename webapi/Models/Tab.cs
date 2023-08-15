namespace webapi.Models
{
    public class Tab
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string? Name { get; set; }
        public DateTime TimeStamp { get; set; }

        public Tab() { }
    }
}
