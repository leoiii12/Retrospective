using System;
using System.Collections.Generic;
using System.Text;
using Retrospective.Boards;

namespace Retrospective.Events
{
    public class BoardItem_Update
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public BoardItemType Type { get; set; }
    }
}
