using System.Numerics;

namespace Ambient.Backend.Mathematics;

public readonly record struct Angle
{
	public readonly float Degrees;
	public readonly float Radians;

	public static readonly Angle Zero = new(0f, 0f);

	private Angle(float degrees, float radians)
	{
		Degrees = degrees;
		Radians = radians;
	}

	public static Angle FromDegrees(float degrees)
	{
		float radians = DegreesToRadians(degrees);
		return new(degrees, radians);
	}

	public static Angle FromRadians(float radians)
	{
		float degrees = RadiansToDegrees(radians);
		return new(degrees, radians);
	}

	public static Angle FromVector(Vector2 vector)
	{
		float radians = MathF.Atan2(vector.Y, vector.X);
		float degrees = RadiansToDegrees(radians);

		return new(degrees, radians);
	}

	public static float DegreesToRadians(float degrees)
	{
		return degrees * MathF.PI / 180f;
	}

	public static float RadiansToDegrees(float radians)
	{
		return radians * (180f / MathF.PI);
	}

	public static Angle operator +(Angle left, Angle right)
	{
		float degrees = left.Degrees + right.Degrees;
		float radians = left.Radians + right.Radians;

		return new(degrees, radians);
	}

	public static Angle operator -(Angle left, Angle right)
	{
		float degrees = left.Degrees - right.Degrees;
		float radians = left.Radians - right.Radians;

		return new(degrees, radians);
	}
}
