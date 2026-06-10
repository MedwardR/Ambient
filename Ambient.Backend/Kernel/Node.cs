namespace Ambient.Backend.Kernel;

/// <summary>
/// An individual element designed to exist as part of a greater ecosystem.
/// </summary>
public abstract class Node
{
	/// <summary>
	/// The central ecosystem the current node belongs to.
	/// </summary>
	public World? Root { get; internal set; }

	/// <summary>
	/// Processes updates for the current node, using <paramref name="deltaTime"/>
	/// to remain frame-rate independent.
	/// </summary>
	/// <param name="deltaTime">
	/// The amount of time since the last update, in seconds.
	/// </param>
	public virtual void Update(float deltaTime) { }
}
