using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using PusherServer;
using Retrospective.Boards.Interfaces;
using Retrospective.Common;
using Retrospective.Events;
using Retrospective.Functions.Dtos;

namespace Retrospective.Functions
{
    public class JoinBoardFunction : IFunction<JoinBoardInput, JoinBoardOutput>
    {
        private readonly IBoardManager _boardManager;
        private readonly IPusher _pusher;

        public JoinBoardFunction(
            IPusher pusher,
            IBoardManager boardManager)
        {
            _pusher = pusher;
            _boardManager = boardManager;
        }

        public async Task<JoinBoardOutput> InvokeAsync(JoinBoardInput input, TraceWriter log)
        {
            var board = await _boardManager.GetAsync(input.BoardId, input.Password) ?? throw new Exception("The board does not exist.");

            var clientId = Guid.NewGuid().ToString();

            var triggerResult = await _pusher.TriggerAsync(board.ToString(), Board_Join.EventName, new Board_Join {ClientId = clientId});
            if (triggerResult.StatusCode != HttpStatusCode.OK) log.Error($"Cannot publish {Board_Join.EventName}. {JsonConvert.SerializeObject(triggerResult)}");

            return new JoinBoardOutput
            {
                Channel = board.ToString(),
                ClientId = clientId
            };
        }
    }
}