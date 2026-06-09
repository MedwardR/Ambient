using System.Numerics;
using System.Windows;

namespace Ambient.Frontend.WindowsHybrid.Utilities;

public static class Dimensions
{
	public static Vector2 GetActualSize(this FrameworkElement element)
	{
		float width = (float)element.ActualWidth;
		float height = (float)element.ActualHeight;

		return new(width, height);
	}
}
