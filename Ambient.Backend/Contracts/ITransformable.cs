using Ambient.Backend.Features;

namespace Ambient.Backend.Contracts;

public interface ITransformable
{
	LinearTransform Transform { get; }
}
