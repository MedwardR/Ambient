using Ambient.Backend.Threading;

namespace Ambient.Backend.Kernel;

public abstract class Node
{
	public bool Initialized { get; private set; } = false;

	public List<Node> Nodes { get; } = [];

	internal void ComposeInternal()
	{
		var children = Compose();

		if (Nodes.Count > 0)
		{
			var missing = children.Except(Nodes);
			Nodes.AddRange(missing);
		}
		else Nodes.AddRange(children);

		foreach (var n in children)
		{
			n.ComposeInternal();
		}
		Initialized = true;
	}

	internal void UpdateInternal(float deltaTime, SyncSystem sync)
	{
		EarlyUpdate(deltaTime);
		EarlyUpdate(deltaTime, sync);

		foreach (var n in Nodes)
		{
			n.UpdateInternal(deltaTime, sync);
		}
		Update(deltaTime);
		Update(deltaTime, sync);
	}

	protected abstract IEnumerable<Node> Compose();

	protected virtual void EarlyUpdate(float deltaTime) { }

	protected virtual void EarlyUpdate(float deltaTime, SyncSystem sync) { }

	protected virtual void Update(float deltaTime) { }

	protected virtual void Update(float deltaTime, SyncSystem sync) { }
}
