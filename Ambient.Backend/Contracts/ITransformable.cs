using Ambient.Backend.Geometry;

namespace Ambient.Backend.Contracts;

public interface ITransformable
{
	LinearTransform Transform { get; }
}
