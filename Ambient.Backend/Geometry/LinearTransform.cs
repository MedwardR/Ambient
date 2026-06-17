using System.Numerics;
using Ambient.Backend.Geometry;

namespace Ambient.Backend.Geometry;

public class LinearTransform
{
	public Vector2 Position { get; set; } = Vector2.Zero;

	public Angle Rotation { get; set; } = Angle.Zero;

	public Vector2 Scale { get; set; } = Vector2.One;

	public bool FlipX { get; set; }

	public bool FlipY { get; set; }
}
