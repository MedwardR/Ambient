using Ambient.Backend.Events;
using Ambient.Backend.Kernel;

namespace Ambient.Backend.Diagnostics;

/// <summary>
/// A node for monitoring and reporting the frame rate within an update cycle.
/// </summary>
public class FrameRateMonitor(double intervalSeconds) : Node
{
	protected bool _running = false;
	protected int _frameCount = 0;
	protected float _totalSeconds = 0;

	/// <summary>
	/// An event that is raised when the target <see cref="Interval">interval</see>
	/// has elapsed.
	/// </summary>
	public event EventHandler<FrameRateEventArgs>? Tick;

	/// <summary>
	/// The interval at which to report the sampled frame rate.
	/// </summary>
	public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(intervalSeconds);

	/// <summary>
	/// Starts or resumes monitoring the frame rate.
	/// </summary>
	public void Start() => _running = true;

	/// <summary>
	/// Pauses monitoring the frame rate. Resume with <see cref="Start"/>.
	/// </summary>
	public void Pause() => _running = false;

	/// <summary>
	/// Stops monitoring the frame rate and resets.
	/// </summary>
	public void Stop()
	{
		_running = false;
		_frameCount = 0;
		_totalSeconds = 0;
	}

	/// <summary>
	/// Resets and starts monitoring the frame rate.
	/// </summary>
	public void Restart()
	{
		_running = true;
		_frameCount = 0;
		_totalSeconds = 0;
	}

	public override void Update(float deltaTime)
	{
		if (_running)
		{
			float intervalSeconds = (float)Interval.TotalSeconds;

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

	/// <summary>
	/// Returns the current average number of frames per second.
	/// </summary>
	public float AverageFramesPerSecond()
	{
		if (_totalSeconds != 0f)
		{
			return _frameCount / _totalSeconds;
		}
		else return 0f;
	}

	/// <summary>
	/// Initializes a new frame rate monitor and starts monitoring the frame rate.
	/// </summary>
	public static FrameRateMonitor StartNew(double intervalSeconds)
	{
		var monitor = new FrameRateMonitor(intervalSeconds);
		monitor.Start();

		return monitor;
	}
}
