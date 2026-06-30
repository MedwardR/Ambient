using System.Drawing;
using System.Numerics;
using System.Windows;

namespace Ambient.Frontend.WindowsHybrid.Extensions;

public static class Dimensions
{
	public static Vector2 Center(this System.Windows.Size size)
	{
		float x = (float)size.Width / 2f;
		float y = (float)size.Height / 2f;

		return new(x, y);
	}

	public static Rect ToRect(this Rectangle rectangle)
	{
		double x = rectangle.X;
		double y = rectangle.Y;
		double width = rectangle.Width;
		double height = rectangle.Height;

		return new(x, y, width, height);
	}
}
