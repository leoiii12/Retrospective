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
    public class GiveSyncFunction : IFunction<GiveSyncInput>
    {
        private readonly IBoardManager _boardManager;
        private readonly IPusher _pusher;

        public GiveSyncFunction(
            IBoardManager boardManager,
            IPusher pusher)
        {
            _boardManager = boardManager;
            _pusher = pusher;
        }

        public async Task InvokeAsync(GiveSyncInput input, TraceWriter log)
        {
            var board = await _boardManager.GetAsync(input.BoardId, input.Password);
            if (board == null) throw new UserFriendlyException("The board does not exist.");

            var triggerResult = await _pusher.TriggerAsync(board.ToString(), "Board-GiveSync", new Board_GiveSync {ClientId = input.ClientId, BoardItems = input.BoardItems});
            if (triggerResult.StatusCode != HttpStatusCode.OK) throw new ApplicationException("Cannot publish \"Board-GiveSync\". " + JsonConvert.SerializeObject(triggerResult));
        }
    }
}