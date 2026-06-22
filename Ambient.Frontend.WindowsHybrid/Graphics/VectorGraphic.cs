using Ambient.Backend.Geometry;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public sealed class VectorGraphic : GraphicWindow
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
