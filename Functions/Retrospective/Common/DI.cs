using System;
using Retrospective.Common;

namespace Retrospective
{
    public static class DI
    {
        public static readonly IServiceProvider Container = new ContainerBuilder().RegisterModule(new CoreAppModule()).Build();
    }
}