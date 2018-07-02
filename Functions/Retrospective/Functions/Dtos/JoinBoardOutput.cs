using System;

namespace Retrospective.Functions
{
    public class JoinBoardOutput
    {
        public string Channel { get; set; }

        public string ClientId { get; set; } = Guid.NewGuid().ToString();
    }
}