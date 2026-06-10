using System.Numerics;
using System.Windows.Forms;

namespace Ambient.Frontend.WindowsHybrid.Utilities;

public static class ScreenInformation
{
	public static Vector2 GetMousePosition()
	{
		var position = Cursor.Position;
		return new(position.X, position.Y);
	}
}
