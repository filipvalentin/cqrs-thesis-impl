namespace Lunatic.Application.Utils.Services {
	public interface IEventQueueService {
		void Enqueue<T>(T item);
		T? Dequeue<T>();
		uint GetEventCount<T>();

	}
}
