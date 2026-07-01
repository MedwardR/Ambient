using System;
using System.Numerics;
using System.Windows.Input;
using Ambient.Frontend.WindowsHybrid.Contracts;
using Ambient.Frontend.WindowsHybrid.Utilities;

namespace Ambient.Frontend.WindowsHybrid.Input;

public class MouseDragController : IDisposable
{
	protected readonly IActor _actor;

	public event EventHandler? DraggingStarted;
	public event EventHandler? DraggingEnded;

	public bool IsDragging { get; protected set; }
	public Vector2 DragOffset { get; protected set; }

	public bool Enabled { get; set; } = false;

	public MouseDragController(IActor actor)
	{
		_actor = actor;

		var element = _actor.Graphics.Element;

		element.MouseLeftButtonDown += OnMouseDown;
		element.MouseMove += OnMouseMove;
		element.MouseLeftButtonUp += OnMouseUp;
		element.LostMouseCapture += OnMouseUp;
	}

	public virtual void Enable() => Enabled = true;

	public virtual void Disable() => Enabled = false;

	protected virtual void OnMouseDown(object sender, MouseButtonEventArgs e)
	{
		if (Enabled && !IsDragging)
		{
			var cursor = ScreenInformation.GetMousePosition();

			IsDragging = true;
			DragOffset = _actor.Transform.Position - cursor;

			_actor.Graphics.Element.CaptureMouse();

			DraggingStarted?.Invoke(this, EventArgs.Empty);
		}
	}

	protected virtual void OnMouseMove(object sender, MouseEventArgs e)
	{
		if (IsDragging)
		{
			if (Enabled)
			{
				var cursor = ScreenInformation.GetMousePosition();
				_actor.Transform.Position = cursor + DragOffset;
			}
			else OnMouseUp(sender, e);
		}
	}

	protected virtual void OnMouseUp(object sender, MouseEventArgs e)
	{
		if (IsDragging)
		{
			try
			{
				IsDragging = false;
				DraggingEnded?.Invoke(this, EventArgs.Empty);
			}
			finally
			{
				_actor.Graphics.Element.ReleaseMouseCapture();
			}
		}
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
			var element = _actor.Graphics.Element;

			element.MouseLeftButtonDown -= OnMouseDown;
			element.MouseMove -= OnMouseMove;
			element.MouseLeftButtonUp -= OnMouseUp;
			element.LostMouseCapture -= OnMouseUp;
		}
	}
}
