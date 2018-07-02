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
using Retrospective.Functions.Dtos;

namespace Retrospective.Functions
{
    public static class JoinBoardHttpTrigger
    {
        [FunctionName("JoinBoard")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req, TraceWriter log)
        {
            log.Info("JoinBoard function processed a request...");

            var inputString = await req.ReadAsStringAsync();
            var input = JsonConvert.DeserializeObject<JoinBoardInput>(inputString);

            var board = await DI.Container.GetService<IFunction<JoinBoardInput, Board>>().InvokeAsync(input);

            return Output.Ok(new
            {
                Channel = board.ToString()
            });
        }
    }
}