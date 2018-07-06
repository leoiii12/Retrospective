using System;
using Retrospective.Boards;

namespace Retrospective.Events
{
    public class BoardItem_Create
    {
        public const string EventName = "BoardItem_Create";
        
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public BoardItemType Type { get; set; }
    }
}