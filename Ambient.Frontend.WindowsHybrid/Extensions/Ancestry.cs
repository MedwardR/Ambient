using System.Collections.Generic;
using System.Windows.Forms;

namespace Ambient.Frontend.WindowsHybrid.Extensions;

public static class Ancestry
{
	public static IEnumerable<Control> Collect(this Control parent)
	{
		yield return parent;

		foreach (Control control in parent.Controls)
		{
			var collection = Collect(control);

			foreach (Control c in collection)
			{
				yield return c;
			}
		}
	}
}
