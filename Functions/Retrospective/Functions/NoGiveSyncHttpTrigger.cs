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
    public static class NoGiveSyncHttpTrigger
    {
        [FunctionName("NoGiveSync")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req, TraceWriter log)
        {
            log.Info("NoGiveSync function processed a request...");

            var inputString = await req.ReadAsStringAsync();
            var input = JsonConvert.DeserializeObject<NoGiveSyncInput>(inputString);

            try
            {
                await DI.Container.GetService<IFunction<NoGiveSyncInput>>().InvokeAsync(input, log);
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

            return Output.Ok();
        }
    }
}