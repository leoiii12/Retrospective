namespace Retrospective.Boards
{
    public class BoardItemDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public BoardItemType Type { get; set; }
        public string DateTime { get; set; }
    }
}