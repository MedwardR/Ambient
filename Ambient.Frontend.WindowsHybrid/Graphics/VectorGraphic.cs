using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ambient.Frontend.WindowsHybrid.Contracts;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public sealed class VectorGraphic : IGraphic
{
	public Canvas Canvas { get; }

	FrameworkElement IGraphic.Element => Canvas;

	public VectorGraphic()
	{
		Canvas = new()
		{
			Background = Brushes.Transparent,
		};
	}
}
