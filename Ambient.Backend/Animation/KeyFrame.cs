namespace Ambient.Backend.Animation;

public readonly record struct KeyFrame<T>
{
	public readonly T Value;

	public readonly TimeSpan Duration;

	public KeyFrame(T value, double durationSeconds)
	{
		Value = value;
		Duration = TimeSpan.FromSeconds(durationSeconds);
	}

	public KeyFrame(T value, TimeSpan duration)
	{
		Value = value;
		Duration = duration;
	}
}
