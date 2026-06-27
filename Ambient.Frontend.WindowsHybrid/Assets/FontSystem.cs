using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using Ambient.Backend.Assets;
using Ambient.Frontend.WindowsHybrid.Extensions;

namespace Ambient.Frontend.WindowsHybrid.Assets;

public class FontSystem(AssetSystem assets)
{
	protected readonly AssetSystem _assets = assets;

	protected readonly PrivateFontCollection _collection = new();

	protected readonly ConcurrentDictionary<string, int> _mappings = new(StringComparer.OrdinalIgnoreCase);

	public FontFamily Load(string path)
	{
		string key = _assets.Resolve(path);

		int Factory(string path)
		{
			_collection.AddFontFile(path);

			return _collection.Families.Length - 1;
		}
		int index = _mappings.GetOrAdd(key, Factory);

		return _collection.Families[index];
	}

	public void ApplyTo(Form form)
	{
		var fonts = _collection.Families.ToDictionary(family => family.Name);

		var enumerable = Ancestry.Collect(form);

		foreach (var control in enumerable)
		{
			var old = control.Font;
			var key = old.OriginalFontName;

			if (!string.IsNullOrWhiteSpace(key))
			{
				if (fonts.TryGetValue(key, out var family))
				{
					float emSize = old.Size;
					var style = old.Style;
					var unit = old.Unit;
					byte gdiCharSet = old.GdiCharSet;
					bool gdiVerticalFont = old.GdiVerticalFont;

					control.Font = new(family, emSize, style, unit, gdiCharSet, gdiVerticalFont);
				}
			}
		}
	}
}
