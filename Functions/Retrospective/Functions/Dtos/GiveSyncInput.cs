using System.Collections.Generic;
using Retrospective.Boards;

namespace Retrospective.Functions.Dtos
{
    public class GiveSyncInput : BoardDto
    {
        public string AskForSyncClientId { get; set; }

        public ICollection<BoardItemDto> BoardItems { get; set; }
    }
}