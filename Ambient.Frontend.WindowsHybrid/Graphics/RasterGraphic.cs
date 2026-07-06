using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ambient.Frontend.WindowsHybrid.Contracts;
using Ambient.Frontend.WindowsHybrid.Sprites;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public class RasterGraphic : IGraphic
{
	public Image Image { get; }

	FrameworkElement IGraphic.Element => Image;

	public RasterGraphic()
	{
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
	}

	public void Use(Sprite sprite) => Image.Source = sprite.Source;
}
