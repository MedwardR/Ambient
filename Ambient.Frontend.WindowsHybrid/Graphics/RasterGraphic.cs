using System.Windows.Controls;
using System.Windows.Media;
using Ambient.Backend.Features;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public sealed class RasterGraphic : WindowsGraphic
{
	public Image Image { get; }

	public RasterGraphic(LinearTransform transform) : base(transform)
	{
		Title = "Ambient Raster Graphic";
		Image = new()
		{
			Source = null,
			Stretch = Stretch.None,
		};
		RenderOptions.SetBitmapScalingMode(
			Image,
			BitmapScalingMode.Linear
		);
		GraphicElement = Image;
	}
}
