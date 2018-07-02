using System.Threading.Tasks;

namespace Retrospective.Common
{
    public interface IFunction<in TInput, TOutput>
    {
        Task<TOutput> InvokeAsync(TInput input);
    }

    public interface IFunction<in TInput>
    {
        Task InvokeAsync(TInput input);
    }
}