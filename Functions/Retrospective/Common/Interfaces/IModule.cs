using Microsoft.Extensions.DependencyInjection;

namespace Retrospective.Common
{
    public interface IModule
    {
        void Load(IServiceCollection services);
    }
}