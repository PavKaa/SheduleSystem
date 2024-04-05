using Domain.Entity;
using ForumService.Handlers;
using ForumService.SocketsManagers;
using Service.Interface;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ForumService.Extensions
{
	public static class SocketsExtension
	{
		public static void AddWebSocketsManager(this IServiceCollection services)
		{
			services.AddSingleton<ConnectionsManager>();

			//services.AddScoped<SocketsMessageHandler>();
			services.AddScoped<SocketsHandler<Message>, SocketsMessageHandler>(serviceProvider =>
			{
				var messageService = serviceProvider.GetRequiredService<IMessageService>();
				var connectionsManager = serviceProvider.GetRequiredService<ConnectionsManager>();

				return new SocketsMessageHandler(connectionsManager, messageService);
			});

			//foreach (var type in Assembly.GetEntryAssembly().ExportedTypes) 
			//{
			//	if(type.GetTypeInfo().BaseType == typeof(SocketsHandler<>))
			//	{
			//		services.AddSingleton(type);
			//	}
			//}
		}
	}
}
