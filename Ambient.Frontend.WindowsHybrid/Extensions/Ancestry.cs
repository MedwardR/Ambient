using System.Collections.Generic;
using System.Windows.Forms;

namespace Ambient.Frontend.WindowsHybrid.Extensions;

public static class Ancestry
{
	public static IEnumerable<Control> Collect(this Control root)
	{
		var stack = new Stack<Control>();
		stack.Push(root);

		while (stack.Count > 0)
		{
			var current = stack.Pop();
			yield return current;

			for (int index = 0; index < current.Controls.Count; index++)
			{
				var child = current.Controls[index];
				stack.Push(child);
			}
		}
	}
}
