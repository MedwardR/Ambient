using System.Collections.ObjectModel;

namespace Ambient.Backend.Kernel;

public class World
{
	public ObservableCollection<Node> Nodes { get; } = [];

	public void Update(float deltaTime)
	{
		foreach (var n in Nodes)
		{
			n.Update(deltaTime);
		}
	}
}
