using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Vereesa.Core.Extensions;
using Vereesa.Data.Interfaces;
using Vereesa.Data.Models.Statistics;
using Vereesa.Core.Infrastructure;
using System.ComponentModel;
using Discord;

namespace Vereesa.Core.Services
{
	public class FlagService : BotServiceBase
	{
		private readonly IRepository<Statistics> _statRepository;
		private Statistics GetFlags() => _statRepository.FindById("flags") ??
			new Statistics { Id = "flags" };
		private readonly ILogger<FlagService> _logger;

		public FlagService(DiscordSocketClient discord, IRepository<Statistics> statRepository,
			ILogger<FlagService> logger)
			: base(discord)
		{
			_statRepository = statRepository;
			_logger = logger;
		}

		[OnCommand("!flag set")]
		[WithArgument("countryName", 0)]
		[WithArgument("flagEmoji", 1)]
		[Authorize("Guild Master")]
		[Description("Sets a flag for the specified country.")]
		[CommandUsage("`!flag set <flag emoji> <country name>`")]
		public async Task EvaluateMessageAsync(IMessage message, string flagEmoji, string countryName) =>
			await message.Channel.SendMessageAsync(SetFlag(flagEmoji, countryName));

		private string SetFlag(string flagEmoji, string countryName)
		{
			if (string.IsNullOrWhiteSpace(flagEmoji) || string.IsNullOrWhiteSpace(countryName))
			{
				return "Please specify a flag emoji and a country name.";
			}

			var flags = GetFlags();
			flags.Upsert(countryName.ToTitleCase(), flagEmoji);
			_statRepository.AddOrEdit(flags);

			return $"OK! {countryName.ToTitleCase()} now has the flag {flagEmoji}!";
		}
	}
}