using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace ForumService.SocketsManagers
{
	public class ConnectionsManager
	{
		private ConcurrentDictionary<long, List<WebSocket>> connections = new ConcurrentDictionary<long, List<WebSocket>>();

		public void AddSocket(WebSocket socket, long topicId)
		{
			if(!connections.ContainsKey(topicId))
			{
				connections.TryAdd(topicId, new List<WebSocket>());
			}
			
			connections[topicId].Add(socket);
		}

		public async Task RemoveSocketAsync(WebSocket socket, long topicId) 
		{
			if (connections.ContainsKey(topicId) && connections[topicId].Remove(socket))
			{
				await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket connection closed", CancellationToken.None);
			}
		}

		public List<WebSocket>? GetSocketsByTopic(long topicId)
		{
			connections.TryGetValue(topicId, out List<WebSocket>? sockets);
			return sockets;
		}

		public ConcurrentDictionary<long, List<WebSocket>> GetAllConnections()
		{
			return connections;
		}
	}
}
