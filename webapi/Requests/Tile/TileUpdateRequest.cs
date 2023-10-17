namespace webapi.Requests.Tile
{
    public class TileUpdateRequest
    {
        public string? Name { get; set; }
        public byte? Position { get; set; }

        public bool isEmpty()
        {
            return Name == null && Position == null;
        }
    }
}
