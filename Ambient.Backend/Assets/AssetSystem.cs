using System.Collections.Concurrent;
using Ambient.Backend.Contracts;

namespace Ambient.Backend.Assets;

public class AssetSystem
{
	protected readonly ConcurrentDictionary<string, byte[]> _cache = new(StringComparer.OrdinalIgnoreCase);

	protected static string ApplicationRoot { get; } = AppContext.BaseDirectory;

	public string AssetsRoot { get; init; }

	public AssetSystem() => AssetsRoot = string.Empty;

	public AssetSystem(string root) => AssetsRoot = root;

	public T Load<T>(string path) where T : IAsset, new()
	{
		byte[] buffer = Load(path);

		var asset = new T();
		asset.Load(buffer);

		return asset;
	}

	public byte[] Load(string path)
	{
		string key = Resolve(path);

		return _cache.GetOrAdd(key, File.ReadAllBytes);
	}

	public virtual string Resolve(string relativePath)
	{
		string normalized = relativePath.Replace('\\', '/').Trim();

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
