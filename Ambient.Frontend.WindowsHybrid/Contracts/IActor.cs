using Ambient.Backend.Contracts;

namespace Ambient.Frontend.WindowsHybrid.Contracts;

public interface IActor : ITransformable
{
	IGraphic Graphics { get; }
}
