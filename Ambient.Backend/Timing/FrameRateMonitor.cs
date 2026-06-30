using Ambient.Backend.Events;
using Ambient.Backend.Kernel;

namespace Ambient.Backend.Timing;

public class FrameRateMonitor(float intervalSeconds) : Node
{
	protected readonly Cooldown _cooldown = new(intervalSeconds);
	protected int _frameCount = 0;

	public event EventHandler<FrameRateEventArgs>? Tick;

	public void Start() => _cooldown.Start();
	public void Pause() => _cooldown.Pause();
	public void Stop() => _cooldown.Stop();
	public void Restart() => _cooldown.Restart();

	public override void Update(float deltaTime)
	{
		if (_cooldown.IsRunning)
		{
			_frameCount++;

			if (_cooldown.Tick())
			{
				float fps = AverageFramesPerSecond();
				var e = new FrameRateEventArgs(fps);

				Tick?.Invoke(this, e);

				_cooldown.Restart();
				_frameCount = 0;
			}
		}
	}

	public float AverageFramesPerSecond()
	{
		if (_cooldown.TotalSeconds != 0f)
		{
			return _frameCount / _cooldown.TotalSeconds;
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
