using System;

namespace Retrospective.Common
{
    // https://blog.mexia.com.au/dependency-injections-on-azure-functions-v2
    public interface IContainerBuilder
    {
        IContainerBuilder RegisterModule(IModule module = null);

        IServiceProvider Build();
    }
}