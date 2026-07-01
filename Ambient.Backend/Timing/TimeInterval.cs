using Ambient.Backend.Kernel;

namespace Ambient.Backend.Timing;

public class TimeInterval(float intervalSeconds) : Node
{
	public bool IsRunning { get; set; } = false;
	public float IntervalSeconds { get; set; } = intervalSeconds;
	public float TotalSeconds { get; protected set; } = 0f;

	public virtual void Start() => IsRunning = true;

	public virtual void Pause() => IsRunning = false;

	public virtual void Stop()
	{
		IsRunning = false;
		TotalSeconds = 0f;
	}

	public virtual void Restart()
	{
		IsRunning = true;
		TotalSeconds = 0f;
	}

	public virtual bool Tick()
	{
		if (IntervalSeconds > 0 && TotalSeconds >= IntervalSeconds)
		{
			TotalSeconds -= IntervalSeconds;

			return true;
		}
		else return false;
	}

	protected override void EarlyUpdate(float deltaTime)
	{
		if (IsRunning)
		{
			TotalSeconds += deltaTime;
		}
	}

	public static TimeInterval StartNew(float intervalSeconds)
	{
		var cooldown = new TimeInterval(intervalSeconds);
		cooldown.Start();

		return cooldown;
	}

	protected override IEnumerable<Node> Compose() => [];
}
