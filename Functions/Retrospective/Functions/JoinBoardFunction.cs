using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using PusherServer;
using Retrospective.Boards;
using Retrospective.Boards.Interfaces;
using Retrospective.Common;
using Retrospective.Events;
using Retrospective.Functions.Dtos;

namespace Retrospective.Functions
{
    public class JoinBoardFunction : IFunction<JoinBoardInput, Board>
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

        public async Task<Board> InvokeAsync(JoinBoardInput input, TraceWriter log)
        {
            var board = await _boardManager.GetAsync(input.BoardId, input.Password);
            if (board == null) throw new Exception("The board does not exist.");

            var triggerResult = await _pusher.TriggerAsync(board.ToString(), "Board-Join", new Board_Join());
            if (triggerResult.StatusCode != HttpStatusCode.OK) log.Error(JsonConvert.SerializeObject(triggerResult));

            return board;
        }
    }
}