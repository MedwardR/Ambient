using Ambient.Backend.Geometry;
using Ambient.Backend.Kernel;
using Ambient.Frontend.WindowsHybrid.Contracts;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public abstract class Actor<T> : Node, IActor where T : IGraphic, new()
{
	public LinearTransform Transform { get; } = new();

	public T Graphics { get; } = new();

	IGraphic IActor.Graphics => Graphics;
}
