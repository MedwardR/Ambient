using System.Numerics;

namespace Ambient.Backend.Features;

public class LinearTransform
{
	public Vector2 Position { get; set; } = Vector2.Zero;

	/// <summary>
	/// Rotation in degrees. Positive values rotate clockwise, negative values rotate counterclockwise.
	/// </summary>
	public float Rotation { get; set; } = 0f;

	public Vector2 Scale { get; set; } = Vector2.One;
}
