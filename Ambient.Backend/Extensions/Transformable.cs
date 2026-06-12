using System.Numerics;
using Ambient.Backend.Contracts;
using Ambient.Backend.Mathematics;

namespace Ambient.Backend.Extensions;

public static class Transformable
{
	public static void MoveTowards(this ITransformable node, Vector2 target, float distance)
	{
		var position = node.Transform.Position;

		if (position != target)
		{
			var difference = target - position;

			var direction = Vector2.Normalize(difference);
			var movement = direction * distance;

			if (difference.Length() >= movement.Length())
			{
				node.Transform.Position += movement;
			}
			else node.Transform.Position = target;
		}
	}

	public static void PointTowards(this ITransformable node, Vector2 target)
	{
		var position = node.Transform.Position;

		if (position != target)
		{
			node.Transform.Rotation = Angle.FromVector(target - position);
		}
	}
}
