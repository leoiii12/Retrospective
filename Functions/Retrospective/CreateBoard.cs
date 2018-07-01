using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Retrospective.Boards;
using Retrospective.Common;

namespace Retrospective
{
    public static class CreateBoard
    {
        [FunctionName("CreateBoard")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req, TraceWriter log)
        {
            log.Info("CreateBoard function processed a request...");

            var inputString = await req.ReadAsStringAsync();
            var input = JsonConvert.DeserializeObject<CreateBoardInput>(inputString);

            var boardManager = DI.Container.GetService<IBoardManager>();

            var board = await boardManager.CreateAsync(input.BoardId);
            if (board == null)
            {
                log.Error("Cannot create a new board.");
                return Output.InternalError();
            }

            return Output.Ok(board.ToDto());
        }

        private class CreateBoardInput
        {
            public string BoardId { get; set; }
        }
    }
}