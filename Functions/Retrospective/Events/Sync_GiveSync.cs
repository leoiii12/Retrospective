using System.Collections.Generic;
using Retrospective.Boards;

namespace Retrospective.Events
{
    public class Sync_GiveSync
    {
        public const string EventName = "Sync_GiveSync";

        public string AskForSyncClientId { get; set; }

        public ICollection<BoardItemDto> BoardItems { get; set; }
    }
}