using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PusherServer;
using Retrospective.Boards;
using Retrospective.Boards.Interfaces;
using Retrospective.Functions;
using Retrospective.Functions.Dtos;

namespace Retrospective.Common
{
    public class CoreAppModule : Module
    {
        public override void Load(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var pusher = new Pusher(
                config["PusherAppId"],
                config["PusherAppKey"],
                config["PusherAppSecret"],
                new PusherOptions
                {
                    Cluster = "ap1",
                    Encrypted = true
                });

            var boardManager = new BoardManager(config["AppStorage"]);

            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<IPusher>(pusher);
            services.AddSingleton<IBoardManager>(boardManager);

            services.AddTransient<IFunction<CreateBoardInput, Board>, CreateBoardFunction>();
            services.AddTransient<IFunction<JoinBoardInput, Board>, JoinBoardFunction>();
            services.AddTransient<IFunction<CreateBoardItemInput>, CreateBoardItemFunction>();
            services.AddTransient<IFunction<UpdateBoardItemInput>, UpdateBoardItemFunction>();
        }
    }
}