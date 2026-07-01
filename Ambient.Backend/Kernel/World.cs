using System.Diagnostics;
using Ambient.Backend.Management;

namespace Ambient.Backend.Kernel;

public class World
{
	private readonly SyncSystem _sync;
	private readonly Thread _updateThread;
	private volatile bool _running;

	public List<Node> Nodes { get; }

	public double FramesPerSecond { get; set; }
	public double MaxDeltaTime { get; set; }

	public World(SynchronizationContext foreground)
	{
		_sync = new(foreground);
		_updateThread = new(Loop)
		{
			IsBackground = true,
		};
		_running = false;

		Nodes = [];
		FramesPerSecond = 60.0;
		MaxDeltaTime = 1.0;
	}

	public T Singleton<T>() => Nodes.OfType<T>().Single();

	public void StartThread()
	{
		_running = true;
		_updateThread.Start();
	}

	public void StopThread()
	{
		_running = false;
		_updateThread.Join();
	}

	protected virtual void Loop()
	{
		var sw = new Stopwatch();

		while (_running)
		{
			float deltaTime = (float)Math.Min(sw.Elapsed.TotalSeconds, MaxDeltaTime);
			sw.Restart();

			foreach (var n in Nodes)
			{
				if (!n.Initialized)
				{
					n.ComposeInternal();
				}
				n.UpdateInternal(deltaTime, _sync);
			}
			_sync.Flush();

			double frameTime = 1.0 / FramesPerSecond;
			double remaining = frameTime - sw.Elapsed.TotalSeconds;
			int milliseconds = (int)(remaining * 1000);

			if (milliseconds > 0)
			{
				Thread.Sleep(milliseconds);
			}
		}
	}
}
