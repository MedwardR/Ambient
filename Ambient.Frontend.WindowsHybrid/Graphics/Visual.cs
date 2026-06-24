using System.Numerics;
using System.Windows.Media;
using Ambient.Backend.Geometry;
using Ambient.Backend.Kernel;
using Ambient.Frontend.WindowsHybrid.Contracts;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public abstract class Visual<T> : Node, IVisual where T : IGraphic, new()
{
	public LinearTransform Transform { get; } = new();

	public T Graphics { get; } = new();

	IGraphic IVisual.Graphics => Graphics;

	public Matrix GetRenderMatrix()
	{
		var m = Matrix.Identity;

		if (Transform.Scale != Vector2.One || Transform.FlipX || Transform.FlipY)
		{
			double x = Transform.FlipX ? -Transform.Scale.X : Transform.Scale.X;
			double y = Transform.FlipY ? -Transform.Scale.Y : Transform.Scale.Y;

			m.Scale(x, y);
		}
		if (Transform.Rotation != Angle.Zero)
		{
			m.Rotate(Transform.Rotation.Degrees);
		}
		if (Transform.Position != Vector2.Zero)
		{
			double x = Transform.Position.X - Graphics.Element.ActualWidth / 2.0;
			double y = Transform.Position.Y - Graphics.Element.ActualHeight / 2.0;

			m.Translate(x, y);
		}
		return m;
	}
}
