using Ambient.Backend.Threading;

namespace Ambient.Backend.Kernel;

public abstract class Node
{
	public List<Node> Nodes { get; } = [];

	internal void UpdateInternal(float deltaTime, SyncSystem sync)
	{
		foreach (var n in Nodes)
		{
			n.UpdateInternal(deltaTime, sync);
		}
		Update(deltaTime);
		Update(deltaTime, sync);
	}

	public virtual void Update(float deltaTime) { }

	public virtual void Update(float deltaTime, SyncSystem sync) { }
}
