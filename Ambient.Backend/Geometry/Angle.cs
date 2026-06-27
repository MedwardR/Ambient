using System.Numerics;

namespace Ambient.Backend.Geometry;

public readonly record struct Angle
{
	public readonly float Degrees;
	public readonly float Radians;

	public static Angle Zero => new(0f, 0f);

	public static Angle Pi => new(180f, MathF.PI);

	private Angle(float degrees, float radians)
	{
		Degrees = degrees;
		Radians = radians;
	}

	public static Angle FromDegrees(float degrees)
	{
		float radians = ToRadians(degrees);
		return new(degrees, radians);
	}

	public static Angle FromRadians(float radians)
	{
		float degrees = ToDegrees(radians);
		return new(degrees, radians);
	}

	public static Angle FromVector(Vector2 vector)
	{
		float radians = MathF.Atan2(vector.Y, vector.X);
		float degrees = ToDegrees(radians);

		return new(degrees, radians);
	}

	public static float ToRadians(float degrees)
	{
		return degrees * MathF.PI / 180f;
	}

	public static float ToDegrees(float radians)
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
