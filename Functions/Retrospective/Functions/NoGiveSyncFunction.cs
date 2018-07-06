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
    public class NoGiveSyncFunction : IFunction<NoGiveSyncInput>
    {
        private readonly IBoardManager _boardManager;
        private readonly IPusher _pusher;

        public NoGiveSyncFunction(
            IPusher pusher,
            IBoardManager boardManager)
        {
            _pusher = pusher;
            _boardManager = boardManager;
        }

        public async Task InvokeAsync(NoGiveSyncInput input, TraceWriter log)
        {
            var board = await _boardManager.GetAsync(input.BoardId, input.Password) ?? throw new UserFriendlyException("The board does not exist.");

            var triggerResult = await _pusher.TriggerAsync(board.ToString(), Sync_NoGiveSync.EventName, new Sync_NoGiveSync());
            if (triggerResult.StatusCode != HttpStatusCode.OK) throw new ApplicationException($"Cannot publish {Sync_NoGiveSync.EventName}. {JsonConvert.SerializeObject(triggerResult)}");
        }
    }
}