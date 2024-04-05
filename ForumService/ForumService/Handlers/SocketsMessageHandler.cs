using Domain.Entity;
using ForumService.SocketsManagers;
using Newtonsoft.Json;
using Service.Interface;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace ForumService.Handlers
{
	public class SocketsMessageHandler : SocketsHandler<Message>
	{
		private readonly IMessageService _messageService;

        public SocketsMessageHandler(ConnectionsManager connectionsManager, IMessageService messageService) : base(connectionsManager)
        {
            _messageService = messageService;
        }

		public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
		{
			var response = await _messageService.CreateAsync(buffer);

            if (response.StatusCode != HttpStatusCode.OK)
            {
				return;
            }

			await SendMessageToAllInTopic(response.Data, response.Data.TopicId);
        }

		public override async Task Send(WebSocket socket, Message message)
		{
			if (socket.State != WebSocketState.Open)
			{
				return;
			}

			string json = JsonConvert.SerializeObject(message);
			byte[] buffer = Encoding.UTF8.GetBytes(json);

			await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length),
								   WebSocketMessageType.Text, true, CancellationToken.None);
		}

		public override async Task SendMessageToAllInTopic(Message message, long topicId)
		{
			var sockets = _connectionsManager.GetSocketsByTopic(topicId);

			if(sockets != null)
			{
				foreach (var socket in sockets)
				{
					await Send(socket, message);
				}
			}
		}

		public override async Task CheckConnections(/*object state*/)
		{
			foreach(var pair in _connectionsManager.GetAllConnections())
			{
				foreach(var socket in  pair.Value)
				{
					try
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

						if (result.MessageType == WebSocketMessageType.Text)
						{
							await Receive(socket, result, message.ToArray());
						}
						else if (result.MessageType == WebSocketMessageType.Close)
						{
							await OnDisconnected(socket, pair.Key);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine($"WebSocket error: {ex.Message}");
					}
				}
			}
		}

		public override async Task CheckForClosedConnections()
		{
			foreach (var pair in _connectionsManager.GetAllConnections())
			{
				foreach (var socket in pair.Value)
				{
					try
					{
						if (socket.State == WebSocketState.Closed || socket.State == WebSocketState.Aborted)
						{
							await OnDisconnected(socket, pair.Key);
						}
					}
					catch (Exception ex) 
					{
						Console.WriteLine($"WebSocket error: {ex.Message}");
					}
				}
			}
		}
	}
}
