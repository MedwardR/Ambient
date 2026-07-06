using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows.Media;
using Ambient.Backend.Kernel;
using Ambient.Backend.Threading;

namespace Ambient.Frontend.WindowsHybrid.Audio;

public class Boombox : Node, IReadOnlyDictionary<string, Uri>, IDisposable
{
	protected Dictionary<string, Uri> _source;
	protected Dictionary<string, MediaPlayer> _sounds;

	protected bool _running;
	protected bool _looping;

	protected bool _switched;
	protected string? _current;
	protected MediaPlayer? _player;

	public Boombox()
	{
		var comparer = StringComparer.OrdinalIgnoreCase;

		_source = [];
		_sounds = new(comparer);

		_running = true;
		_looping = false;

		_switched = false;
		_current = null;
		_player = null;
	}

	public virtual void Add(string sound, Uri uri)
	{
		if (!string.IsNullOrWhiteSpace(sound))
		{
			string trimmed = sound.Trim();
			var player = new MediaPlayer();

			player.MediaFailed += OnMediaFailed;
			player.MediaEnded += OnMediaEnded;
			player.Open(uri);

			_source.Add(trimmed, uri);
			_sounds.Add(trimmed, player);
		}
		else throw new ArgumentException("Name cannot be empty", nameof(sound));
	}

	public virtual void Play(string sound, bool looping)
	{
		string trimmed = sound.Trim();

		if (!string.Equals(trimmed, _current, StringComparison.OrdinalIgnoreCase))
		{
			if (_sounds.ContainsKey(trimmed))
			{
				_current = trimmed;
				_looping = looping;
				_switched = true;
			}
			else throw new ArgumentException($"Sound not registered: {trimmed}", nameof(sound));
		}
	}

	protected override void Update(float deltaTime, SyncSystem sync)
	{
		if (_running && _switched && _current is not null)
		{
			_switched = false;

			var updated = _sounds[_current];
			var old = Interlocked.Exchange(ref _player, updated);

			void ExecuteChanges()
			{
				old?.Stop();
				updated.Play();
			}
			sync.Schedule(ExecuteChanges);
		}
	}

	protected virtual void OnMediaFailed(object? sender, ExceptionEventArgs e)
	{
		string message = e.ErrorException.Message;
		string detailMessage = e.ErrorException.ToString();

		Debug.Fail(message, detailMessage);
	}

	protected virtual void OnMediaEnded(object? sender, EventArgs e)
	{
		if (_looping && _player is not null)
		{
			_player.Position = TimeSpan.Zero;
			_player.Play();
		}
	}

	public Uri this[string key] => _source[key];
	public int Count => _sounds.Count;

	public IEnumerable<string> Keys => _source.Keys;
	public IEnumerable<Uri> Values => _source.Values;

	public bool ContainsKey(string key) => _source.ContainsKey(key);

	public bool TryGetValue(string key, [MaybeNullWhen(false)] out Uri value)
	{
		return _source.TryGetValue(key, out value);
	}

	public IEnumerator<KeyValuePair<string, Uri>> GetEnumerator() => _source.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_running = false;

			foreach (var player in _sounds.Values)
			{
				player.MediaFailed -= OnMediaFailed;
				player.MediaEnded -= OnMediaEnded;
				player.Close();
			}
		}
	}

	protected override IEnumerable<Node> Compose() => [];
}
