using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Text;
using Ambient.Backend.Assets;

namespace Ambient.Frontend.WindowsHybrid.Assets;

public class FontSystem(AssetSystem assets)
{
	protected readonly AssetSystem _assets = assets;

	protected readonly PrivateFontCollection _collection = new();

	protected readonly ConcurrentDictionary<string, int> _mappings = new(StringComparer.OrdinalIgnoreCase);

	public FontFamily Load(string path)
	{
		string key = _assets.Resolve(path);

		int index = _mappings.GetOrAdd(key, LoadFont);

		return _collection.Families[index];
	}

	protected int LoadFont(string path)
	{
		_collection.AddFontFile(path);

		return _collection.Families.Length - 1;
	}
}
