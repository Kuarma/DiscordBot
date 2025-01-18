using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DiscordBot;

public class Worker : IHostedService
{
    private readonly DiscordSocketClient _client;
    
    public Worker(ILogger<Worker> logger, DiscordSocketClient client)
    {
        _client = client;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.Log += LogDiscord;
        
        string token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
        if (token != null)
        {
            _client.LoginAsync(TokenType.Bot, token);
        }
        await _client.StartAsync();
        Log.Information("Discord bot started");
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private Task LogDiscord(LogMessage log)
    {
        switch (log.Severity)
        {
            case LogSeverity.Critical:
            case LogSeverity.Error:
                Log.Error(log.Exception, log.Message);
                break;
            case LogSeverity.Warning:
                Log.Warning(log.Exception, log.Message);
                break;
            case LogSeverity.Info:
                Log.Information(log.Exception, log.Message);
                break;
            case LogSeverity.Verbose:
            case LogSeverity.Debug:
                Log.Debug(log.Exception, log.Message);
                break;
        }
        return Task.CompletedTask;
    }
}