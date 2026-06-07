using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ambient.Backend.Features;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public class RasterGraphic : WindowsGraphic
{
	public Image Image { get; }

	public RasterGraphic(LinearTransform transform) : base(transform)
	{
		Image = new()
		{
			Source = null,
			Stretch = Stretch.Fill,
		};
		RenderOptions.SetBitmapScalingMode(
			Image,
			BitmapScalingMode.Linear
		);
		Window.Content = Image;
	}

	protected override void OnRendering(object? sender, EventArgs e)
	{
		if (Image.Source is ImageSource source)
		{
			Window.Width = source.Width * Transform.Scale.X;
			Window.Height = source.Height * Transform.Scale.Y;
		}
		base.OnRendering(sender, e);
	}
}
