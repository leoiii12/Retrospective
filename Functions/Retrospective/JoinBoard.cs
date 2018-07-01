using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PusherServer;
using Retrospective.Boards;
using Retrospective.Common;
using Retrospective.Events;

namespace Retrospective
{
    public static class JoinBoard
    {
        [FunctionName("JoinBoard")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req, TraceWriter log, ExecutionContext context)
        {
            log.Info("JoinBoard function processed a request...");

            var inputString = await req.ReadAsStringAsync();
            var input = JsonConvert.DeserializeObject<JoinBoardInput>(inputString);

            var pusher = DI.Container.GetService<IPusher>();
            var boardManager = DI.Container.GetService<IBoardManager>();

            var board = await boardManager.GetAsync(input.BoardId, input.Password);
            if (board == null) return Output.Error("The board does not exist.");

            var clientId = Guid.NewGuid().ToString();

            var triggerResult = await pusher.TriggerAsync(board.ToString(), "Board-Join", new Board_Join {ClientId = clientId});
            if (triggerResult.StatusCode != HttpStatusCode.OK) log.Error(JsonConvert.SerializeObject(triggerResult));

            return Output.Ok(new
            {
                Channel = board.ToString(),
                ClientId = clientId
            });
        }

        private class JoinBoardInput : BoardDto
        {
        }
    }
}