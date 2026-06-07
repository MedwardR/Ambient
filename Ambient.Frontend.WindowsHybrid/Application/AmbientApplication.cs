using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Ambient.Backend.Kernel;

namespace Ambient.Frontend.WindowsHybrid.Application;

public class AmbientApplication : System.Windows.Application
{
	public event EventHandler? ManageRequested;

	public string Name { get; init; }

	public NotifyIcon TrayIcon { get; }
	public World World { get; }

	private readonly Thread _updateThread;
	private volatile bool _running;

	private const int TARGET_FPS = 60;
	private const double TARGET_FRAME_SECONDS = 1.0 / TARGET_FPS;

	public AmbientApplication()
	{
		Name = "Ambient Application";
		TrayIcon = new()
		{
			Icon = SystemIcons.Application,
			Text = Name,
			Visible = true,
		};
		World = new();

		_updateThread = new(UpdateLoop) { IsBackground = true };
		_running = true;
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		var menu = new ContextMenuStrip();
		OnMenuCreating(menu);

		TrayIcon.ContextMenuStrip = menu;
		TrayIcon.DoubleClick += ManageClicked;

		_updateThread.Start();
	}

	protected void UpdateLoop()
	{
		var sw = new Stopwatch();

		while (_running)
		{
			float deltaTime = (float)sw.Elapsed.TotalSeconds;
			sw.Restart();

			World.Update(deltaTime);

			double remaining = TARGET_FRAME_SECONDS - sw.Elapsed.TotalSeconds;
			int milliseconds = (int)(remaining * 1000);

			if (milliseconds > 0)
			{
				Thread.Sleep(milliseconds);
			}
		}
	}

	protected virtual void OnMenuCreating(ContextMenuStrip menu)
	{
		menu.Items.Add("Manage", null, ManageClicked);
		menu.Items.Add("Exit", null, ExitClicked);
	}

	private void ManageClicked(object? sender, EventArgs e)
	{
		ManageRequested?.Invoke(this, EventArgs.Empty);
	}

	private void ExitClicked(object? sender, EventArgs e)
	{
		try
		{
			TrayIcon.Dispose();
		}
		finally
		{
			Shutdown();
		}
	}

	protected override void OnExit(ExitEventArgs e)
	{
		try
		{
			_running = false;
			_updateThread.Join();
		}
		finally
		{
			base.OnExit(e);
		}
	}
}
