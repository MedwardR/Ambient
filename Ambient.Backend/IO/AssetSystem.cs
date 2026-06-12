using System.Collections.Concurrent;
using Ambient.Backend.Contracts;
using Ambient.Backend.Kernel;

namespace Ambient.Backend.IO;

public class AssetSystem(string? root = default) : Node
{
	protected readonly ConcurrentDictionary<string, byte[]> _cache = [];

	protected static string ApplicationRoot { get; } = AppContext.BaseDirectory;

	public string AssetsRoot { get; init; } = root ?? string.Empty;

	public T LoadAsset<T>(string path) where T : IAsset, new()
	{
		string key = Resolve(path);
		byte[] buffer = _cache.GetOrAdd(key, File.ReadAllBytes);

		var asset = new T();
		asset.Load(buffer);

		return asset;
	}

	public virtual string Resolve(string path)
	{
		string normalized = path.Replace('\\', '/');
		string combined = Path.Combine(ApplicationRoot, AssetsRoot, normalized);

		return Path.GetFullPath(combined);
	}
}
