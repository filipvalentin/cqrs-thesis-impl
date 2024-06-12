using Lunatic.Application.Utils.Services;
using Lunatic.Domain.Primitives;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lunatic.Infrastructure.Services {
	public class FailedEventProcessorService<TEvent>(
		IEventQueueService queueService, 
		IServiceProvider serviceProvider,
		ILogger<FailedEventProcessorService<TEvent>> logger) : BackgroundService where TEvent : IDomainEvent {

		private readonly IEventQueueService queueService = queueService;
		private readonly IServiceProvider serviceProvider = serviceProvider;
		private readonly ILogger<FailedEventProcessorService<TEvent>> logger = logger;

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			while (!stoppingToken.IsCancellationRequested) {
				try {
					var eventNotification = queueService.Dequeue<TEvent>();
					if (eventNotification != null) {
						logger.LogInformation("Processing failed domain event for {Name}", this.GetType().Name);
						using var scope = serviceProvider.CreateScope();
						var handler = scope.ServiceProvider.GetRequiredService<INotificationHandler<TEvent>>();
						await handler.Handle(eventNotification, stoppingToken);
					}
				}
				catch (Exception ex) {
					logger.LogError(ex, "Error processing failed domain event for {Name}", this.GetType().Name);
				}

				if (queueService.GetEventCount<TEvent>() == 0) {
					await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
				}
			}
		}
	}

}
