namespace webapi.Requests.Tile
{
    public class TileUpdateRequest
    {
        public Guid? TabId { get; set; }
        public string? Name { get; set; }
        public ushort? X { get; set; }
        public ushort? Y { get; set; }
        public ushort? H { get; set; }
        public ushort? W { get; set; }

        public bool isEmpty()
        {
            return TabId == null && Name == null && X == null && Y == null && H == null && W == null;
        }
    }
}
