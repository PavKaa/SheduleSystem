using ForumService.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace ForumService.Middlewares
{
	public class WebSocketsMiddleware
	{
		private readonly RequestDelegate next;

		public SocketsHandler SocketsHandler { get; private set; }

		public WebSocketsMiddleware(RequestDelegate next, SocketsHandler socketsHandler)
		{
			this.next = next;
			SocketsHandler = socketsHandler;
		}

		public async Task Invoke(HttpContext context)
		{
			if (context.WebSockets.IsWebSocketRequest)
			{
				//var socket = await context.WebSockets.AcceptWebSocketAsync();

				//await SocketsHandler.OnConnected(socket);

				//await Receive(socket, async (result, buffer) =>
				//{
				//	if (result.MessageType == WebSocketMessageType.Text)
				//	{
				//		await SocketsHandler.Receive(socket, result, buffer);
				//	}
				//	else if (result.MessageType == WebSocketMessageType.Close)
				//	{
				//		await SocketsHandler.OnDisconnected(socket);
				//	}
				//});
			}

			await next.Invoke(context);
		}

		private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> messageHandler)
		{
			var buffer = new byte[4096];
			var message = new List<byte>();
			WebSocketReceiveResult result;

			do
			{
				result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

				if (result.MessageType == WebSocketMessageType.Text)
				{
					message.AddRange(buffer.Take(result.Count));
				}
			} 
			while (!result.EndOfMessage);

			await SocketsHandler.Receive(socket, result, message.ToArray());
		}
	}
}
