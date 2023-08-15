using webapi.Models;

namespace webapi.DTO.Tabs
{
    public class TabAllInfo
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime TimeStamp { get; set; }

        public TabAllInfo() { }

        public TabAllInfo(Tab tab)
        {
            Id = tab.Id;
            Name = tab.Name;
            TimeStamp = tab.TimeStamp;
        }
    }
}
