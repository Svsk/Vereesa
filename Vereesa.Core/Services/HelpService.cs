using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Vereesa.Core.Extensions;
using Vereesa.Core.Infrastructure;

namespace Vereesa.Core.Services
{
	public class HelpService : BotServiceBase
	{
		public HelpService(DiscordSocketClient discord) : base(discord) { }

		[OnCommand("!help")]
		[Description("Prints helpful information about commands.")]
		[AsyncHandler]
		public async Task HandleMessage(IMessage message)
		{
			var services = BotServices.GetBotServices();
			foreach (var service in services) 
			{
				var helpMessage = "";
				var invokableMethods = service.GetMethodsWithAttribute<OnCommandAttribute>();

				foreach (var method in invokableMethods) 
				{
					var obsoleteAttribute = method.GetCustomAttribute<ObsoleteAttribute>();
					var commandAttribute = method.GetCustomAttribute<OnCommandAttribute>();
					var descriptionAttribute = method.GetCustomAttribute<DescriptionAttribute>();
					var usageAttribute = method.GetCustomAttribute<CommandUsageAttribute>();
					var restrictionAttributes = method.GetCustomAttributes<AuthorizeAttribute>();

					if (obsoleteAttribute != null) 
					{
						continue;
					}
					
					helpMessage += $"`{commandAttribute.Command}` ";

					if (restrictionAttributes.Any()) 
					{
						helpMessage += "- 🔒 ";
					}

					if (descriptionAttribute != null) 
					{
						helpMessage += $"- {descriptionAttribute.Description} ";
					}

					if (usageAttribute != null) 
					{
						helpMessage += $"- {usageAttribute.UsageDescription} ";
					}

					helpMessage += "\n";
				}

				if (invokableMethods.Any() && !string.IsNullOrWhiteSpace(helpMessage))
				{
					helpMessage += "\n";
					await message.Channel.SendMessageAsync(helpMessage);
					await Task.Delay(50);
				}
			}
		}
	}

	public static class TypeExtensions 
	{
		public static MethodInfo[] GetMethodsWithAttribute<T>(this Type type) where T : Attribute
		{
			return type.GetMethods().Where(method => method.GetCustomAttribute<T>() != null).ToArray();
		}
	}
}