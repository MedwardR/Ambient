namespace Ambient.Kernel;

public class World
{
	public List<Node> Nodes { get; } = [];

	public void Update(float deltaTime)
	{
		foreach (var n in Nodes)
		{
			n.Update(deltaTime);
		}
	}
}
