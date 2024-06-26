﻿using Domain.Entity;
using ForumService.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace ForumService.Middlewares
{
	public class WebSocketsMiddleware
	{
		private readonly RequestDelegate next;
		//private readonly SocketsHandler<Message> _socketsHandler;

		public WebSocketsMiddleware(RequestDelegate next/*, SocketsHandler<Message> socketsHandler*/)
		{
			this.next = next;
			//_socketsHandler = socketsHandler;
		}

		public async Task Invoke(HttpContext context, SocketsHandler<Message> socketsHandler)
		{

			if (/*context.User != null && context.User.Identity.IsAuthenticated &&*/ context.WebSockets.IsWebSocketRequest)
			{
				var socket = await context.WebSockets.AcceptWebSocketAsync();

				if (long.TryParse(context.Request.Query["topicId"].ToString(), out long topicId))
				{
					await socketsHandler.OnConnected(socket, topicId);
				}

				await Receive(socket, async (result, buffer) =>
				{
					if (result.MessageType == WebSocketMessageType.Text)
					{
						await socketsHandler.Receive(socket, result, buffer);
					}
					else if (result.MessageType == WebSocketMessageType.Close)
					{
						await socketsHandler.OnDisconnected(socket, topicId);
					}
				});
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

				if(result.EndOfMessage)
				{
					messageHandler(result, message.ToArray());
					message.Clear();
				}
			}
			while (socket.State == WebSocketState.Open);

			//await socketsHandler.Receive(socket, result, message.ToArray());
		}
	}
}
