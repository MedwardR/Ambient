using System.Windows.Controls;
using System.Windows.Media;
using Ambient.Backend.Features;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public sealed class VectorGraphic : WindowsGraphic
{
	public Canvas Canvas { get; }

	public VectorGraphic(LinearTransform transform) : base(transform)
	{
		Title = "Ambient Vector Graphic";
		Canvas = new()
		{
			Background = Brushes.Transparent,
		};
		GraphicElement = Canvas;
	}
}
