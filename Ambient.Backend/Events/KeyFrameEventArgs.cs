using Ambient.Backend.Animation;

namespace Ambient.Backend.Events;

public class KeyFrameEventArgs<T>(KeyFrame<T> frame) : EventArgs
{
	public KeyFrame<T> Frame { get; } = frame;
}
