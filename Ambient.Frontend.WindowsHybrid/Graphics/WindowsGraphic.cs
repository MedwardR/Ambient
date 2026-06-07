using System;
using System.Windows;
using System.Windows.Media;
using Ambient.Backend.Features;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public abstract class WindowsGraphic
{
	protected LinearTransform Transform { get; }
	protected Window Window { get; }

	public string Title
	{
		get => Window.Title;
		set => Window.Title = value;
	}

	public Brush Background
	{
		get => Window.Background;
		set => Window.Background = value;
	}

	public bool Visible
	{
		get => Window.Visibility == Visibility.Visible;
		set => Window.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
	}

	public WindowsGraphic(LinearTransform transform)
	{
		Transform = transform;
		Window = new()
		{
			Title = "Ambient Raster Graphic",
			Width = 400,
			Height = 400,
			WindowStyle = WindowStyle.None,

			AllowsTransparency = true,
			Background = Brushes.Transparent,
			Visibility = Visibility.Visible,
		};
		CompositionTarget.Rendering += OnRendering;
	}

	protected virtual void OnRendering(object? sender, EventArgs e)
	{
		Window.Left = Transform.Position.X - Window.Width / 2.0;
		Window.Top = Transform.Position.Y - Window.Height / 2.0;
	}
}
