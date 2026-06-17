using Ambient.Backend.Events;
using Ambient.Backend.Kernel;
using Ambient.Backend.Threading;

namespace Ambient.Backend.Animation;

public class Animator<T>(KeyFrame<T>[] frames) : Node
{
	protected bool _running = false;
	protected int _frameIndex = 0;
	protected float _frameSeconds = 0f;

	public event EventHandler<KeyFrameEventArgs<T>>? FrameChanged;

	public KeyFrame<T>[] Frames { get; } = frames;
	public int FrameIndex => _frameIndex;

	public bool Looping { get; set; } = true;

	public void Start() => _running = true;

	public void Pause() => _running = false;

	public void Stop()
	{
		_running = false;
		_frameIndex = 0;
		_frameSeconds = 0f;
	}

	public void Restart()
	{
		_running = true;
		_frameIndex = 0;
		_frameSeconds = 0f;
	}

	public KeyFrame<T> Frame() => Frames[_frameIndex];

	public override void Update(float deltaTime, SyncSystem sync)
	{
		if (_running)
		{
			_frameSeconds += deltaTime;

			float durationSeconds = (float)Frames[_frameIndex].Duration.TotalSeconds;

			while (_frameSeconds >= durationSeconds)
			{
				_frameSeconds -= durationSeconds;
				_frameIndex++;

				if (_frameIndex >= Frames.Length)
				{
					if (!Looping)
					{
						_running = false;
						_frameIndex = Frames.Length - 1;
					}
					else _frameIndex = 0;
				}
				if (_running)
				{
					var frame = Frames[_frameIndex];
					durationSeconds = (float)frame.Duration.TotalSeconds;

					var e = new KeyFrameEventArgs<T>(frame);

					sync.Schedule(() =>
					{
						FrameChanged?.Invoke(this, e);
					});
				}
				else break;
			}
		}
	}
}
