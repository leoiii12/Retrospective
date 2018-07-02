using System.Collections.Generic;
using Retrospective.Boards;

namespace Retrospective.Events
{
    public class Board_GiveSync
    {
        public string ClientId { get; set; }

        public ICollection<BoardItemDto> BoardItems { get; set; }
    }
}