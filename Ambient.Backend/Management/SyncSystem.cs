using System.Collections.Concurrent;

namespace Ambient.Backend.Management;

public class SyncSystem(SynchronizationContext context)
{
	protected readonly ConcurrentQueue<Action> _queue = [];

	public void Schedule(Action callback)
	{
		_queue.Enqueue(callback);
	}

	public void Flush()
	{
		if (!_queue.IsEmpty)
		{
			if (SynchronizationContext.Current == context)
			{
				ExecuteScheduled(null);
			}
			else context.Send(ExecuteScheduled, null);
		}
	}

	protected void ExecuteScheduled(object? state)
	{
		while (_queue.TryDequeue(out var callback))
		{
			callback();
		}
	}
}
