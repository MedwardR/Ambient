using System.Numerics;
using Ambient.Backend.Mathematics;

namespace Ambient.Backend.Features;

public class LinearTransform
{
	public Vector2 Position { get; set; } = Vector2.Zero;

	public Angle Rotation { get; set; } = Angle.Zero;

	public Vector2 Scale { get; set; } = Vector2.One;

	public void Flip(Axis axis)
	{
		float x = axis.HasFlag(Axis.X) ? -Scale.X : Scale.X;
		float y = axis.HasFlag(Axis.Y) ? -Scale.Y : Scale.Y;

		Scale = new(x, y);
	}
}
