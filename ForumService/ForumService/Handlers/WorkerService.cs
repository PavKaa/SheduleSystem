using Domain.Entity;
using System.Threading;

namespace ForumService.Handlers
{
	public class WorkerService : IHostedService, IDisposable
	{
		private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public WorkerService(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			Listen(_stoppingCts.Token);
			return Task.CompletedTask;
		}

		public async Task Listen(CancellationToken cancellationToken)
		{
			using(var scope = _serviceScopeFactory.CreateScope())
			{
				var socketsHandler = scope.ServiceProvider.GetRequiredService<SocketsMessageHandler>();

				while (!cancellationToken.IsCancellationRequested)
				{
					await socketsHandler.CheckConnections();
					await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_stoppingCts.Cancel();
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_stoppingCts.Cancel();
		}
	}
}
