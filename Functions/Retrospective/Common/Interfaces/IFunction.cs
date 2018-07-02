using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;

namespace Retrospective.Common
{
    public interface IFunction<in TInput, TOutput>
    {
        Task<TOutput> InvokeAsync(TInput input, TraceWriter log);
    }

    public interface IFunction<in TInput>
    {
        Task InvokeAsync(TInput input, TraceWriter log);
    }
}