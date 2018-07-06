using System;

namespace Retrospective.Functions.Dtos
{
    public class JoinBoardOutput
    {
        public string Channel { get; set; }

        public string ClientId { get; set; } = Guid.NewGuid().ToString();
    }
}