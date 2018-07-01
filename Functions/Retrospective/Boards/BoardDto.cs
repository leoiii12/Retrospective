namespace Retrospective.Boards
{
    public class BoardDto
    {
        private string _boardId;
        public string BoardId
        {
            get => _boardId;
            set => _boardId = value.Trim().Replace(" ", "-");
        }

        public string Password { get; set; }
    }
}