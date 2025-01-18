using Discord.WebSocket;

namespace DiscordBot;

public class GetOrCreateDesignatedDiscordChannel
{
    private readonly DiscordSocketClient _client;
    
    public GetOrCreateDesignatedDiscordChannel(DiscordSocketClient client)
    {
        _client = client;
    }
    
    public SocketTextChannel GetTextChannelIfExists(ulong channelId)
    {
        var guildId = _client.GetGuild(channelId);
        var channel = guildId.TextChannels.Where(x => x.Name == "chatting");
        var socketTextChannels = channel as SocketTextChannel[] ?? channel.ToArray();
        if (socketTextChannels.Any())
        {
            return socketTextChannels.First();
        }
        return CreateTextChannel(guildId).Result;
    }

    private async Task<SocketTextChannel> CreateTextChannel(SocketGuild guildId)
    {
        var newChannel = await guildId.CreateTextChannelAsync("chatting");
        await Task.Delay(500);
        var socketTextChannel = guildId.GetChannel(newChannel.Id) as SocketTextChannel;
        return socketTextChannel;
    }
}