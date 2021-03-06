using Vereesa.Core.Infrastructure;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using System.ComponentModel;

namespace Vereesa.Core.Services
{
	public class PingService : BotServiceBase
	{
		public PingService(DiscordSocketClient discord)
			: base(discord)
		{
		}

		[OnCommand("!ping")]
		[Description("Ping Vereesa to check if she is still alive and accepting commands.")]
		public async Task HandleMessageAsync(IMessage message)
		{
			var responseMessage = await message.Channel.SendMessageAsync($"Pong!");
			var responseTimestamp = responseMessage.Timestamp.ToUnixTimeMilliseconds();
			var messageSentTimestamp = message.Timestamp.ToUnixTimeMilliseconds();
			await responseMessage.ModifyAsync((msg) => 
			{
				msg.Content = $"Pong! (Responded after {responseTimestamp - messageSentTimestamp} ms)"; 
			});
		}
	}
}