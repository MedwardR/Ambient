using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using Ambient.Backend.Features;
using Ambient.Backend.Mathematics;
using Ambient.Frontend.WindowsHybrid.Extensions;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public abstract class WindowsGraphic : IDisposable
{
	public FrameworkElement? GraphicElement { get; init; }

	protected LinearTransform NodeTransform { get; }
	protected MatrixTransform RenderTransform { get; }
	protected Window Window { get; }

	protected bool AllowClosing { get; set; }

	public string Title
	{
		get => Window.Title;
		set => Window.Title = value;
	}

	public bool Visible
	{
		get => Window.Visibility == Visibility.Visible;
		set => Window.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
	}

	public WindowsGraphic(LinearTransform transform)
	{
		var matrix = GetRenderMatrix(transform);

		NodeTransform = transform;
		RenderTransform = new(matrix);
		Window = new()
		{
			Title = "Ambient Graphic",
			WindowStyle = WindowStyle.None,

			AllowsTransparency = true,
			Background = Brushes.Transparent,
			ResizeMode = ResizeMode.NoResize,
			Visibility = Visibility.Visible,
		};
		AllowClosing = false;

		CompositionTarget.Rendering += OnRendering;
		Window.Closing += OnClosing;
	}

	protected virtual void OnRendering(object? sender, EventArgs e)
	{
		if (!ReferenceEquals(Window.Content, GraphicElement))
		{
			Window.Content = GraphicElement;
		}
		if (GraphicElement is not null)
		{
			var transform = NodeTransform;
			var matrix = GetRenderMatrix(transform);

			if (matrix != RenderTransform.Matrix)
			{
				RenderTransform.Matrix = matrix;
			}
			if (!ReferenceEquals(GraphicElement.RenderTransform, RenderTransform))
			{
				GraphicElement.RenderTransform = RenderTransform;
				GraphicElement.RenderTransformOrigin = new Point(0.5, 0.5);
			}
			if (Window.ActualWidth > 0.0 && Window.ActualHeight > 0.0)
			{
				Window.Left = transform.Position.X - Window.ActualWidth / 2.0;
				Window.Top = transform.Position.Y - Window.ActualHeight / 2.0;

				float length = GraphicElement.GetActualSize().Length();
				float scale = MathF.Max(
					MathF.Abs(transform.Scale.X),
					MathF.Abs(transform.Scale.Y)
				);
				Window.Width = length * scale;
				Window.Height = length * scale;
			}
		}
	}

	protected virtual void OnClosing(object? sender, CancelEventArgs e)
	{
		if (!AllowClosing)
		{
			e.Cancel = true;
		}
	}

	protected static Matrix GetRenderMatrix(LinearTransform transform)
	{
		var m = Matrix.Identity;

		if (transform.Scale != Vector2.One || transform.FlipX || transform.FlipY)
		{
			double x = transform.FlipX ? -transform.Scale.X : transform.Scale.X;
			double y = transform.FlipY ? -transform.Scale.Y : transform.Scale.Y;

			m.Scale(x, y);
		}
		if (transform.Rotation != Angle.Zero || transform.FlipX || transform.FlipY)
		{
			double degrees = transform.Rotation.Degrees;
			double angle = transform.FlipX ? degrees + 180.0 : degrees;

			m.Rotate(angle);
		}
		return m;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			AllowClosing = true;
			Window.Close();
		}
	}
}
