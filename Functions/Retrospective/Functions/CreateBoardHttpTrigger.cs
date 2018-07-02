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
    public static class CreateBoardHttpTrigger
    {
        [FunctionName("CreateBoard")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req, TraceWriter log)
        {
            var inputString = await req.ReadAsStringAsync();
            var input = JsonConvert.DeserializeObject<CreateBoardInput>(inputString);

            var createBoardFunction = DI.Container.GetService<IFunction<CreateBoardInput, Board>>();
            var board = await createBoardFunction.InvokeAsync(input);

            return Output.Ok(board.ToDto());
        }
    }
}