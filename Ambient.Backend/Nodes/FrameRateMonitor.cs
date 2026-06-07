using Ambient.Backend.Events;
using Ambient.Backend.Kernel;

namespace Ambient.Backend.Nodes;

public class FrameRateMonitor(double intervalSeconds) : Node
{
	public event EventHandler<FrameRateEventArgs>? Tick;

	public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(intervalSeconds);

	protected bool _running = false;
	protected int _frameCount = 0;
	protected float _totalSeconds = 0;

	public void Start() => _running = true;

	public void Stop() => _running = false;

	public void Reset()
	{
		_running = false;
		_frameCount = 0;
		_totalSeconds = 0;
	}

	public void Restart()
	{
		Reset();
		Start();
	}

	public override void Update(float deltaTime)
	{
		if (_running)
		{
			_frameCount++;
			_totalSeconds += deltaTime;

			if (Interval.TotalSeconds >= 0 && _totalSeconds > Interval.TotalSeconds)
			{
				float fps = AverageFramesPerSecond();
				var e = new FrameRateEventArgs(fps);

				Tick?.Invoke(this, e);
				Restart();
			}
		}
	}

	public float AverageFramesPerSecond()
	{
		if (_totalSeconds != 0f)
		{
			return _frameCount / _totalSeconds;
		}
		else return 0f;
	}

	public static FrameRateMonitor StartNew(double intervalSeconds)
	{
		var monitor = new FrameRateMonitor(intervalSeconds);
		monitor.Start();

		return monitor;
	}
}
