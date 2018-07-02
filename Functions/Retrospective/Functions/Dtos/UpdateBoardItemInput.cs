using Retrospective.Boards;

namespace Retrospective.Functions.Dtos
{
    public class UpdateBoardItemInput : BoardDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public BoardItemType Type { get; set; }
    }
}