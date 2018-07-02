using Retrospective.Boards;

namespace Retrospective.Functions.Dtos
{
    public class CreateBoardItemInput : BoardDto
    {
        public BoardItemType Type { get; set; }
    }
}