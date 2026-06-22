using System.Numerics;
using System.Windows;
using System.Windows.Forms;

namespace Ambient.Frontend.WindowsHybrid.Utilities;

public static class ScreenInformation
{
	public static Vector2 GetMousePosition()
	{
		var position = Cursor.Position;

		return new(position.X, position.Y);
	}

	public static Rect[] GetWorkingAreas()
	{
		var screens = Screen.AllScreens;

		var workingAreas = new Rect[screens.Length];

		for (int index = 0; index < screens.Length; index++)
		{
			var area = screens[index].WorkingArea;

			workingAreas[index] = new(area.X, area.Y, area.Width, area.Height);
		}
		return workingAreas;
	}
}
