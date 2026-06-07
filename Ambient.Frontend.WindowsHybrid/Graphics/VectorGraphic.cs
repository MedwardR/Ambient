using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ambient.Backend.Features;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public class VectorGraphic : WindowsGraphic
{
	public Canvas Canvas { get; }

	public VectorGraphic(LinearTransform transform) : base(transform)
	{
		Canvas = new()
		{
			Background = Brushes.Transparent,
		};
		Window.Content = Canvas;
	}
}
