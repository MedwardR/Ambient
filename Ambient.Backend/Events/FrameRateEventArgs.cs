namespace Ambient.Backend.Events;

public class FrameRateEventArgs(float fps) : EventArgs
{
	public float FramesPerSecond { get; } = fps;
}
