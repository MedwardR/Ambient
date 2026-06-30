using System.Drawing;
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

	public static Rectangle[] GetWorkingAreas()
	{
		var screens = Screen.AllScreens;

		var workingAreas = new Rectangle[screens.Length];

		for (int index = 0; index < screens.Length; index++)
		{
			var area = screens[index].WorkingArea;

			workingAreas[index] = new(area.X, area.Y, area.Width, area.Height);
		}
		return workingAreas;
	}

	public static Rectangle GetCombinedWorkingArea()
	{
		var screens = Screen.AllScreens;

		var bounds = screens[0].WorkingArea;

		for (int i = 1; i < screens.Length; i++)
		{
			var area = screens[i].WorkingArea;

			bounds = Rectangle.Union(bounds, area);
		}
		return bounds;
	}
}
