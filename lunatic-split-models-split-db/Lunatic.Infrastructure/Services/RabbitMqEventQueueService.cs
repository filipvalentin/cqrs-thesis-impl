using Lunatic.Application.Utils.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;

namespace Lunatic.Infrastructure.Services {
	public class RabbitMqEventQueueService : IEventQueueService {
		private readonly IConnection connection;
		private readonly IModel channel;

		public RabbitMqEventQueueService(string hostname) {
			var factory = new ConnectionFactory() { HostName = hostname };
			connection = factory.CreateConnection();
			channel = connection.CreateModel();
		}

		public void Enqueue<T>(T item) {
			var message = JsonConvert.SerializeObject(item);
			var body = Encoding.UTF8.GetBytes(message);
			var queueName = typeof(T).Name;

			channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
			channel.BasicPublish(exchange: string.Empty, routingKey: queueName, basicProperties: null, body: body);
		}

		public T? Dequeue<T>() {
			var queueName = typeof(T).Name;
			channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

			var result = channel.BasicGet(queueName, autoAck: false);
			if (result == null) {
				return default; 
			}

			var body = result.Body.ToArray();
			var message = Encoding.UTF8.GetString(body);
			var item = JsonConvert.DeserializeObject<T>(message);
			channel.BasicAck(deliveryTag: result.DeliveryTag, multiple: false);

			return item;
		}

		public uint GetEventCount<T>() {
			var result = channel.QueueDeclarePassive(typeof(T).Name);
			return result.MessageCount;
		}

		public void Dispose() {
			channel.Dispose();
			connection.Dispose();
		}
	}
}
