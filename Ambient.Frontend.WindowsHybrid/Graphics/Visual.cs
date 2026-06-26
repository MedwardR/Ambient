using Ambient.Backend.Geometry;
using Ambient.Backend.Kernel;
using Ambient.Frontend.WindowsHybrid.Contracts;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public abstract class Visual<T> : Node, IVisual where T : IGraphic, new()
{
	public LinearTransform Transform { get; } = new();

	public T Graphics { get; } = new();

	IGraphic IVisual.Graphics => Graphics;
}
