using System;
using System.Collections.Generic;
using System.Text;
using Retrospective.Boards;

namespace Retrospective.Events
{
    public class BoardItem_Create
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public BoardItemType Type { get; set; }
    }
}
