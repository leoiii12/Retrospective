using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Retrospective.Common;
using Retrospective.Functions.Dtos;

namespace Retrospective.Functions
{
    public static class CreateBoardItemHttpTrigger
    {
        [FunctionName("CreateBoardItem")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req, TraceWriter log)
        {
            var inputString = await req.ReadAsStringAsync();
            var input = JsonConvert.DeserializeObject<CreateBoardItemInput>(inputString);

            await DI.Container.GetService<IFunction<CreateBoardItemInput>>().InvokeAsync(input);

            return Output.Ok();
        }
    }
}