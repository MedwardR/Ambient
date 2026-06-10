using System.Diagnostics;

namespace Ambient.Backend.Kernel;

/// <summary>
/// The primary center for managing state within the engine's ecosystem.
/// Node updates are processed in a continuous cycle, with interval periods
/// calculated from a <see cref="FramesPerSecond">specified target frame-rate</see>.
/// </summary>
public class World
{
	private readonly Thread _updateThread;
	private volatile bool _running;

	protected List<Node> _nodes;

	/// <summary>
	/// The collection of nodes to be enumerated during the update cycle.
	/// </summary>
	public IReadOnlyList<Node> Nodes => _nodes;

	/// <summary>
	/// A number indicating the ideal frame rate of the update cycle (60 by default).
	/// </summary>
	public double FramesPerSecond { get; set; }

	/// <inheritdoc cref="World"/>
	public World()
	{
		_updateThread = new(Loop)
		{
			IsBackground = true,
		};
		_running = false;
		_nodes = [];

		FramesPerSecond = 60.0;
	}

	/// <summary>
	/// Adds a node to be enumerated during the update cycle.
	/// </summary>
	public void AddNode(Node item)
	{
		item.Root = this;
		_nodes.Add(item);
	}

	/// <summary>
	/// Starts the main update cycle on a background thread.
	/// </summary>
	public void StartThread()
	{
		_running = true;
		_updateThread.Start();
	}

	/// <summary>
	/// Stops the main update cycle and joins the corresponding thread.
	/// </summary>
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
			float deltaTime = (float)sw.Elapsed.TotalSeconds;
			sw.Restart();

			foreach (var n in Nodes)
			{
				n.Update(deltaTime);
			}
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
