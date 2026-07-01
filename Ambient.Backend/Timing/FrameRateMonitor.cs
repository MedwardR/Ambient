using Ambient.Backend.Events;
using Ambient.Backend.Kernel;

namespace Ambient.Backend.Timing;

public class FrameRateMonitor(float intervalSeconds) : Node
{
	protected bool _running = false;
	protected int _frameCount = 0;
	protected float _totalSeconds = 0f;

	public event EventHandler<FrameRateEventArgs>? Tick;

	public void Start() => _running = true;

	public void Pause() => _running = false;

	public void Stop()
	{
		_running = false;
		_frameCount = 0;
		_totalSeconds = 0f;
	}

	public void Restart()
	{
		_running = true;
		_frameCount = 0;
		_totalSeconds = 0f;
	}

	public override void Update(float deltaTime)
	{
		if (_running)
		{
			_frameCount++;
			_totalSeconds += deltaTime;

			if (intervalSeconds >= 0 && _totalSeconds > intervalSeconds)
			{
				float fps = AverageFramesPerSecond();
				var e = new FrameRateEventArgs(fps);

				Tick?.Invoke(this, e);

				_frameCount = 0;
				_totalSeconds -= intervalSeconds;
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

	public static FrameRateMonitor StartNew(float intervalSeconds)
	{
		var monitor = new FrameRateMonitor(intervalSeconds);
		monitor.Start();

		return monitor;
	}
}
