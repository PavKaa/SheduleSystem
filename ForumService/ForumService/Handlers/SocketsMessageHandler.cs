using Domain.Entity;
using ForumService.SocketsManagers;
using Newtonsoft.Json;
using Service.Interface;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace ForumService.Handlers
{
	public class SocketsMessageHandler : SocketsHandler
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

		public override async Task Send(WebSocket socket, object obj)
		{
			if (socket.State != WebSocketState.Open || obj is not Message)
			{
				return;
			}

			Message message = (Message)obj;
			string json = JsonConvert.SerializeObject(message);
			byte[] buffer = Encoding.UTF8.GetBytes(json);

			await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length),
								   WebSocketMessageType.Text, true, CancellationToken.None);
		}

		public override async Task SendMessageToAllInTopic(object obj, long topicId)
		{
			var sockets = _connectionsManager.GetSocketsByTopic(topicId);

			if(sockets != null)
			{
				foreach (var socket in sockets)
				{
					await Send(socket, obj);
				}
			}
		}
	}
}
