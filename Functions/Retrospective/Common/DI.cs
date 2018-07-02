using System;

namespace Retrospective.Common
{
    public static class DI
    {
        public static readonly IServiceProvider Container = new ContainerBuilder().RegisterModule(new CoreAppModule()).Build();
    }
}