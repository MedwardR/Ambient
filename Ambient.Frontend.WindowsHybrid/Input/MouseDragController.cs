using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Input;
using Ambient.Backend.Kernel;
using Ambient.Frontend.WindowsHybrid.Contracts;
using Ambient.Frontend.WindowsHybrid.Utilities;

namespace Ambient.Frontend.WindowsHybrid.Input;

public class MouseDragController : Node, IDisposable
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
		element.MouseLeftButtonUp += OnMouseUp;
		element.LostMouseCapture += OnMouseUp;
	}

	public virtual void Enable() => Enabled = true;

	public virtual void Disable() => Enabled = false;

	protected virtual void OnMouseDown(object sender, MouseButtonEventArgs e)
	{
		if (Enabled && !IsDragging) Drag();
	}

	protected virtual void OnMouseUp(object sender, MouseEventArgs e)
	{
		if (IsDragging) Drop();
	}

	protected virtual void Drag()
	{
		var cursor = ScreenInformation.GetMousePosition();

		IsDragging = true;
		DragOffset = _actor.Transform.Position - cursor;

		_actor.Graphics.Element.CaptureMouse();

		DraggingStarted?.Invoke(this, EventArgs.Empty);
	}

	protected override void EarlyUpdate(float deltaTime)
	{
		if (IsDragging)
		{
			if (Enabled)
			{
				var cursor = ScreenInformation.GetMousePosition();

				_actor.Transform.Position = cursor + DragOffset;
			}
			else Drop();
		}
	}

	protected virtual void Drop()
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
			element.MouseLeftButtonUp -= OnMouseUp;
			element.LostMouseCapture -= OnMouseUp;
		}
	}

	protected override IEnumerable<Node> Compose() => [];
}
