using ForumService.SocketsManagers;
using Service.Interface;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace ForumService.Handlers
{
	public abstract class SocketsHandler<T>
	{
		protected readonly ConnectionsManager _connectionsManager;

        public SocketsHandler(ConnectionsManager connectionsManager)
        {
            _connectionsManager = connectionsManager;
        }

        public virtual async Task OnConnected(WebSocket socket, long topicId)
        {
            await Task.Run(() =>
            {
                _connectionsManager.AddSocket(socket, topicId);
            });
        }

        public virtual async Task OnDisconnected(WebSocket socket, long topicId)
        {
            await _connectionsManager.RemoveSocketAsync(socket, topicId);
        }

		public abstract Task Send(WebSocket socket, T message);

        public abstract Task SendMessageToAllInTopic(T message, long topicId);

		public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);

        public abstract Task CheckConnections(/*object state*/);

        public abstract Task CheckForClosedConnections();

	}
}
