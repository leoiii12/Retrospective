using System;
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
    public static class CreateBoardHttpTrigger
    {
        [FunctionName("CreateBoard")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req, TraceWriter log)
        {
            var inputString = await req.ReadAsStringAsync();
            var input = JsonConvert.DeserializeObject<CreateBoardInput>(inputString);

            CreateBoardOutput output;

            try
            {
                output = await DI.Container.GetService<IFunction<CreateBoardInput, CreateBoardOutput>>().InvokeAsync(input, log);
            }
            catch (UserFriendlyException exception)
            {
                return Output.Error(exception.Message);
            }
            catch (Exception exception)
            {
                log.Error(exception.Message, exception);
                return Output.InternalError();
            }

            return Output.Ok(output);
        }
    }
}