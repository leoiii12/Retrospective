using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        private readonly IPusher _pusher;

        public JoinBoardFunction(
            IPusher pusher,
            IBoardManager boardManager,
            ILogger logger)
        {
            _pusher = pusher;
            _boardManager = boardManager;
            _logger = logger;
        }

        public async Task<Board> InvokeAsync(JoinBoardInput input)
        {
            var board = await _boardManager.GetAsync(input.BoardId, input.Password);
            if (board == null) throw new Exception("The board does not exist.");

            var triggerResult = await _pusher.TriggerAsync(board.ToString(), "Board-Join", new Board_Join());
            if (triggerResult.StatusCode != HttpStatusCode.OK) _logger.LogError(JsonConvert.SerializeObject(triggerResult));

            return board;
        }
    }
}