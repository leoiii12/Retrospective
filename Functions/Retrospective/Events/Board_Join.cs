namespace Retrospective.Events
{
    public class Board_Join
    {
        public const string EventName = "Board_Join";

        public string ClientId { get; set; }
    }
}