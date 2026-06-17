using System.Collections.Concurrent;
using Ambient.Backend.Contracts;

namespace Ambient.Backend.IO;

public class AssetSystem
{
	protected readonly ConcurrentDictionary<string, byte[]> _cache = [];

	protected static string ApplicationRoot { get; } = AppContext.BaseDirectory;

	public string AssetsRoot { get; init; }

	public AssetSystem() => AssetsRoot = string.Empty;

	public AssetSystem(string root) => AssetsRoot = root;

	public T LoadAsset<T>(string path) where T : IAsset, new()
	{
		string key = Resolve(path);
		byte[] buffer = _cache.GetOrAdd(key, File.ReadAllBytes);

		var asset = new T();
		asset.Load(buffer);

		return asset;
	}

	public virtual string Resolve(string relativePath)
	{
		string normalized = relativePath.Replace('\\', '/');
		string combined = Path.Combine(ApplicationRoot, AssetsRoot, normalized);
		string path = Path.GetFullPath(combined);

		if (File.Exists(path))
		{
			return path;
		}
		else throw new FileNotFoundException($"Asset file not found: '{path}'", path);
	}

	public virtual void Clear() => _cache.Clear();
}
