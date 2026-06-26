using System.Numerics;
using System.Windows;

namespace Ambient.Frontend.WindowsHybrid.Extensions;

public static class Dimensions
{
	public static Vector2 Center(this Size size)
	{
		float x = (float)size.Width / 2f;
		float y = (float)size.Height / 2f;

		return new(x, y);
	}
}
