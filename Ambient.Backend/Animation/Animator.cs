using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Ambient.Backend.Events;
using Ambient.Backend.Kernel;
using Ambient.Backend.Threading;

namespace Ambient.Backend.Animation;

public class Animator<T>() : Node, IReadOnlyDictionary<string, KeyFrame<T>[]>
{
	protected bool _running = false;
	protected Dictionary<string, KeyFrame<T>[]> _animations = new(StringComparer.OrdinalIgnoreCase);

	protected string? _current;
	protected KeyFrame<T>[] _frames = [];

	protected int _frameIndex = 0;
	protected float _frameSeconds = 0f;

	public event EventHandler<KeyFrameEventArgs<T>>? FrameChanged;

	public bool Looping { get; set; } = true;
	public int FrameIndex => _frameIndex;

	public KeyFrame<T> Frame() => _frames[_frameIndex];

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

	public void Add(string animation, KeyFrame<T>[] frames)
	{
		if (_current is null)
		{
			_current = animation;
			_frames = frames;
		}
		_animations.Add(animation, frames);
	}

	public void Use(string animation)
	{
		var trimmed = animation.Trim();

		if (!string.Equals(_current, trimmed, StringComparison.OrdinalIgnoreCase))
		{
			if (_animations.TryGetValue(trimmed, out var frames))
			{
				_current = trimmed;
				_frames = frames;
				_frameIndex = 0;
				_frameSeconds = 0f;
			}
		}
	}

	public override void Update(float deltaTime, SyncSystem sync)
	{
		if (_running)
		{
			_frameSeconds += deltaTime;

			float durationSeconds = (float)_frames[_frameIndex].Duration.TotalSeconds;

			while (_frameSeconds >= durationSeconds)
			{
				_frameSeconds -= durationSeconds;
				_frameIndex++;

				if (_frameIndex >= _frames.Length)
				{
					if (!Looping)
					{
						_running = false;
						_frameIndex = _frames.Length - 1;
					}
					else _frameIndex = 0;
				}
				if (_running)
				{
					var frame = _frames[_frameIndex];
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

	public KeyFrame<T>[] this[string key] => _animations[key];

	public IEnumerable<string> Keys => _animations.Keys;
	public IEnumerable<KeyFrame<T>[]> Values => _animations.Values;

	public int Count => _animations.Count;

	public bool ContainsKey(string key) => _animations.ContainsKey(key);

	public bool TryGetValue(string key, [MaybeNullWhen(false)] out KeyFrame<T>[] value)
	{
		return _animations.TryGetValue(key, out value);
	}

	public IEnumerator<KeyValuePair<string, KeyFrame<T>[]>> GetEnumerator()
	{
		return _animations.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
