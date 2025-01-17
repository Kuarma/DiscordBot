using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordBot;

public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;
    private readonly DiscordSocketClient _client;
    
    public Worker(ILogger<Worker> logger, DiscordSocketClient client)
    {
        _logger = logger;
        _client = client;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        string token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}