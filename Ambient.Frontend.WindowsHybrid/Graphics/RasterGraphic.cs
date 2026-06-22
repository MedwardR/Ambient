using System.Windows.Controls;
using System.Windows.Media;
using Ambient.Backend.Geometry;
using Ambient.Frontend.WindowsHybrid.Visuals;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public class RasterGraphic : GraphicWindow
{
	public Image Image { get; }

	public RasterGraphic(LinearTransform transform) : base(transform)
	{
		Title = "Ambient Raster Graphic";
		Image = new()
		{
			Source = null,
			Stretch = Stretch.None,
			SnapsToDevicePixels = true,
		};
		RenderOptions.SetBitmapScalingMode(
			Image,
			BitmapScalingMode.Linear
		);
		GraphicElement = Image;
	}

	public void Use(Sprite sprite) => Image.Source = sprite.Source;
}
