using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ambient.Backend.Geometry;
using Ambient.Backend.Kernel;
using Ambient.Frontend.WindowsHybrid.Contracts;
using Ambient.Frontend.WindowsHybrid.Extensions;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public class VirtualViewport
{
	protected readonly World _world;
	protected readonly Rect _bounds;

	protected readonly Canvas _canvas;
	protected readonly Window _window;

	protected readonly Stack<Node> _stack;
	protected readonly HashSet<UIElement> _elements;

	public string Title
	{
		get => _window.Title;
		set => _window.Title = value;
	}

	public VirtualViewport(World world, Rect bounds)
	{
		_world = world;
		_bounds = bounds;
		_canvas = new()
		{
			Background = Brushes.Transparent,
		};
		_window = new()
		{
			Title = "Ambient Application",
			WindowStyle = WindowStyle.None,

			AllowsTransparency = true,
			Background = Brushes.Transparent,
			ResizeMode = ResizeMode.NoResize,
			Visibility = Visibility.Visible,

			Left = bounds.Left,
			Top = bounds.Top,
			Width = bounds.Width,
			Height = bounds.Height,

			Content = _canvas,
		};
		_stack = [];
		_elements = [];

		CompositionTarget.Rendering += OnRendering;
	}

	protected virtual void OnRendering(object? sender, EventArgs e)
	{
		_stack.Clear();

		for (int index = _world.Nodes.Count - 1; index >= 0; index--)
		{
			var n = _world.Nodes[index];
			_stack.Push(n);
		}
		while (_stack.Count > 0)
		{
			var n = _stack.Pop();

			if (n is IVisual v)
			{
				Render(v);
			}
			for (int index = n.Nodes.Count - 1; index >= 0; index--)
			{
				var child = n.Nodes[index];
				_stack.Push(child);
			}
		}
	}

	protected virtual void Render(IVisual v)
	{
		var transform = v.Transform;
		var element = v.Graphics.Element;

		if (_elements.Add(element))
		{
			_canvas.Children.Add(element);
		}
		var matrix = GetRenderMatrix(transform, element);

		if (element.RenderTransform is not MatrixTransform current || current.Matrix != matrix)
		{
			element.RenderTransform = new MatrixTransform(matrix);
		}
	}

	protected virtual Matrix GetRenderMatrix(LinearTransform transform, FrameworkElement element)
	{
		var matrix = Matrix.Identity;
		var center = element.RenderSize.Center();

		matrix.Translate(-center.X, -center.Y);

		if (transform.Scale != Vector2.One || transform.FlipX || transform.FlipY)
		{
			double x = transform.FlipX ? -transform.Scale.X : transform.Scale.X;
			double y = transform.FlipY ? -transform.Scale.Y : transform.Scale.Y;

			matrix.Scale(x, y);
		}
		if (transform.Rotation != Angle.Zero)
		{
			matrix.Rotate(transform.Rotation.Degrees);
		}
		if (transform.Position != Vector2.Zero)
		{
			double x = transform.Position.X - _bounds.X;
			double y = transform.Position.Y - _bounds.Y;

			matrix.Translate(x, y);
		}
		return matrix;
	}
}
