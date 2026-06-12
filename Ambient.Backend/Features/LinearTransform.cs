using System.Numerics;
using Ambient.Backend.Mathematics;

namespace Ambient.Backend.Features;

/// <summary>
/// Represents the position, rotation, and scale of an object.
/// </summary>
public class LinearTransform
{
	/// <summary>
	/// The object's position in space (defaults to <see cref="Vector2.Zero"/>).
	/// </summary>
	public Vector2 Position { get; set; } = Vector2.Zero;

	/// <summary>
	/// The object's rotation around its Z-axis (defaults to <see cref="Angle.Zero"/>).
	/// </summary>
	public Angle Rotation { get; set; } = Angle.Zero;

	/// <summary>
	/// The object's scale (defaults to <see cref="Vector2.One"/>).
	/// </summary>
	public Vector2 Scale { get; set; } = Vector2.One;

	/// <summary>
	/// Determines if the object's X-axis should be inverted.
	/// </summary>
	public bool FlipX { get; set; }

	/// <summary>
	/// Determines if the object's Y-axis should be inverted.
	/// </summary>
	public bool FlipY { get; set; }
}
