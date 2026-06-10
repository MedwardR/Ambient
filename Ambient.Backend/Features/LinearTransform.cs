using System.Numerics;
using Ambient.Backend.Mathematics;

namespace Ambient.Backend.Features;

/// <summary>
/// Represents the
/// <see href="https://en.wikipedia.org/wiki/Transformation_(function)">
/// position, rotation, and scale</see>
/// of an object.
/// </summary>
public class LinearTransform
{
	/// <summary>
	/// The object's position in space (defaults to <see cref="Vector2.Zero"/>).
	/// </summary>
	public Vector2 Position { get; set; } = Vector2.Zero;

	/// <summary>
	/// The object's rotation around it's z-axis (defaults to <see cref="Angle.Zero"/>).
	/// </summary>
	public Angle Rotation { get; set; } = Angle.Zero;

	/// <summary>
	/// The object's scale (defaults to <see cref="Vector2.One"/>).
	/// </summary>
	public Vector2 Scale { get; set; } = Vector2.One;

	/// <summary>
	/// Mirrors the object across the specified axes by inverting
	/// one or both components of the scale property.
	/// </summary>
	public void Flip(Axis axis)
	{
		float x = axis.HasFlag(Axis.X) ? -Scale.X : Scale.X;
		float y = axis.HasFlag(Axis.Y) ? -Scale.Y : Scale.Y;

		Scale = new(x, y);
	}
}
