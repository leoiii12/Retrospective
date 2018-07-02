using Retrospective.Boards;

namespace Retrospective.Events
{
    public class BoardItem_Update : BoardItemDto
    {
        public BoardItem_Update()
        {
            DateTime = System.DateTime.UtcNow.ToString("O");
        }
    }
}