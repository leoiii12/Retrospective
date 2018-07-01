using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Retrospective.Common
{
    public class Output
    {
        public bool Success { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public string DateTime { get; set; } = System.DateTime.UtcNow.ToString("o");

        public static IActionResult Ok()
        {
            return new OkObjectResult(new Output
            {
                Success = true
            });
        }

        public static IActionResult Ok(object data)
        {
            return new OkObjectResult(new Output
            {
                Success = true,
                Data = data
            });
        }

        public static IActionResult Error(string message)
        {
            return new OkObjectResult(new Output
            {
                Success = false,
                Message = message
            });
        }

        public static IActionResult InternalError()
        {
            return new BadRequestObjectResult(new Output
            {
                Success = false
            });
        }
    }
}