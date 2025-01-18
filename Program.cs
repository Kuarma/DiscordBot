using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using Serilog;

namespace DiscordBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("log.txt")
                .WriteTo.Console()
                .CreateLogger();
            
            using IHost host = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(new ChatClient("gpt-4o-mini",Environment.GetEnvironmentVariable("OPENAI_API_KEY")));
                    services.AddSingleton<GetOrCreateDesignatedDiscordChannel>(); 
                    services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                    {
                        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
                    }));
                    services.AddHostedService<Worker>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();
            
            await host.RunAsync();
        }
    }
}

