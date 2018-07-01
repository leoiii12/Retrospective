using Microsoft.Extensions.DependencyInjection;

namespace Retrospective.Common
{
    public class Module : IModule
    {
        public virtual void Load(IServiceCollection services)
        {
        }
    }
}