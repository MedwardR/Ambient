using System;
using System.Drawing;
using System.IO;

namespace Ambient.Frontend.WindowsHybrid.Utilities;

public static class SystemFunctions
{
	public static Icon ExtractApplicationIcon()
	{
		var path = Environment.ProcessPath;

		if (File.Exists(path))
		{
			var ico = Icon.ExtractAssociatedIcon(path);
			return ico ?? SystemIcons.Application;
		}
		else return SystemIcons.Application;
	}
}
