using Retrospective.Boards;

namespace Retrospective.Events
{
    public class BoardItem_Update : BoardItemDto
    {
        public const string EventName = "BoardItem_Update";

        public BoardItem_Update()
        {
            DateTime = System.DateTime.UtcNow.ToString("O");
        }
    }
}