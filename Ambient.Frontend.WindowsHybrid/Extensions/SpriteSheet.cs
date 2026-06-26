using System.Collections.Generic;
using Ambient.Backend.Animation;
using Ambient.Frontend.WindowsHybrid.Assets;

namespace Ambient.Frontend.WindowsHybrid.Extensions;

public static class Spritesheet
{
	public static KeyFrame<Sprite>[] Animate(this Sprite spritesheet, SpriteAnimationTemplate template)
	{
		var sprites = spritesheet.Split(template.FrameWidth, template.FrameHeight);

		return Animate(sprites, template.FrameSeconds);
	}

	public static KeyFrame<Sprite>[] Animate(this Sprite[] spritesheet, float frameSeconds)
	{
		var frames = new KeyFrame<Sprite>[spritesheet.Length];

		for (int index = 0; index < spritesheet.Length; index++)
		{
			var sprite = spritesheet[index];
			frames[index] = new(sprite, frameSeconds);
		}
		return frames;
	}

	public static KeyFrame<Sprite>[] Animate(this ICollection<Sprite> spritesheet, float frameSeconds)
	{
		var frames = new KeyFrame<Sprite>[spritesheet.Count];
		int index = 0;

		foreach (var sprite in spritesheet)
		{
			frames[index++] = new(sprite, frameSeconds);
		}
		return frames;
	}

	public static KeyFrame<Sprite>[] Animate(this IEnumerable<Sprite> spritesheet, float frameSeconds)
	{
		var frames = new List<KeyFrame<Sprite>>();

		foreach (var sprite in spritesheet)
		{
			var item = new KeyFrame<Sprite>(sprite, frameSeconds);
			frames.Add(item);
		}
		return [.. frames];
	}
}

