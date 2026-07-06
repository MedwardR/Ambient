using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Ambient.Backend.Events;
using Ambient.Backend.Kernel;
using Ambient.Backend.Threading;

namespace Ambient.Backend.Animation;

public class Animator<T> : Node, IReadOnlyDictionary<string, KeyFrame<T>[]>
{
	protected Dictionary<string, KeyFrame<T>[]> _animations;

	protected bool _switched;
	protected string? _current;
	protected KeyFrame<T>[] _frames;

	protected bool _running;
	protected int _frameIndex;
	protected float _frameSeconds;

	private const string EMPTY_NAME_EX = "Animation name cannot be empty";
	private const string ZERO_FRAMES_EX = "Animation must have at least one frame";

	public event EventHandler<KeyFrameEventArgs<T>>? FrameChanged;

	public bool Looping { get; set; }

	public string? Current => _current;
	public int FrameIndex => _frameIndex;
	public KeyFrame<T> Frame => _frames[_frameIndex];

	public Animator()
	{
		var comparer = StringComparer.OrdinalIgnoreCase;

		_animations = new(comparer);
		_switched = false;
		_current = null;
		_frames = [];

		_running = false;
		_frameIndex = 0;
		_frameSeconds = 0f;

		Looping = true;
	}

	public virtual void Start() => _running = true;

	public virtual void Pause() => _running = false;

	public virtual void Stop()
	{
		_running = false;
		_frameIndex = 0;
		_frameSeconds = 0f;
	}

	public virtual void Restart()
	{
		_running = true;
		_frameIndex = 0;
		_frameSeconds = 0f;
	}

	public virtual void Add(string animation, KeyFrame<T>[] frames)
	{
		if (string.IsNullOrWhiteSpace(animation))
		{
			throw new ArgumentException(EMPTY_NAME_EX, nameof(animation));
		}
		else if (frames.Length > 0)
		{
			string trimmed = animation.Trim();

			_animations.Add(trimmed, frames);

			if (_current is null)
			{
				UseInternal(trimmed, frames);
			}
		}
		else throw new ArgumentException(ZERO_FRAMES_EX, nameof(frames));
	}

	public virtual void Use(string animation)
	{
		string trimmed = animation.Trim();

		if (!string.Equals(trimmed, _current, StringComparison.OrdinalIgnoreCase))
		{
			if (_animations.TryGetValue(trimmed, out var frames))
			{
				UseInternal(trimmed, frames);
			}
			else throw new ArgumentException($"Animation not found: '{animation}'");
		}
	}

	protected virtual void UseInternal(string animation, KeyFrame<T>[] frames)
	{
		_switched = true;
		_current = animation;
		_frames = frames;
		_frameIndex = 0;
		_frameSeconds = 0f;
	}

	protected override void Update(float deltaTime, SyncSystem sync)
	{
		if (_running)
		{
			if (_switched)
			{
				_switched = false;

				NotifyFrameChanged(_frames[_frameIndex], sync);
			}
			_frameSeconds += deltaTime;

			float frameDuration = _frames[_frameIndex].DurationSeconds;

			while (_frameSeconds >= frameDuration)
			{
				_frameSeconds -= frameDuration;
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

					frameDuration = frame.DurationSeconds;

					NotifyFrameChanged(frame, sync);
				}
				else break;
			}
		}
	}

	protected virtual void NotifyFrameChanged(KeyFrame<T> frame, SyncSystem sync)
	{
		var handler = FrameChanged;

		if (handler is not null)
		{
			var e = new KeyFrameEventArgs<T>(frame);

			void RaiseEvent()
			{
				handler.Invoke(this, e);
			}
			sync.Schedule(RaiseEvent);
		}
	}

	public KeyFrame<T>[] this[string key] => _animations[key];
	public int Count => _animations.Count;

	public IEnumerable<string> Keys => _animations.Keys;
	public IEnumerable<KeyFrame<T>[]> Values => _animations.Values;

	public bool ContainsKey(string key) => _animations.ContainsKey(key);

	public bool TryGetValue(string key, [MaybeNullWhen(false)] out KeyFrame<T>[] value)
	{
		return _animations.TryGetValue(key, out value);
	}

	public IEnumerator<KeyValuePair<string, KeyFrame<T>[]>> GetEnumerator() => _animations.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	protected override IEnumerable<Node> Compose() => [];
}
