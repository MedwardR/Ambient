using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Ambient.Backend.Kernel;

namespace Ambient.Frontend.WindowsHybrid.Application;

public class AmbientApplication : System.Windows.Application
{
	public event EventHandler? ManageRequested;

	public string Name
	{
		get => TrayIcon.Text;
		set => TrayIcon.Text = value;
	}

	public NotifyIcon TrayIcon { get; }

	public World World { get; }

	public AmbientApplication()
	{
		var foreground = new DispatcherSynchronizationContext(Dispatcher);

		TrayIcon = new()
		{
			Text = "Ambient Application",
			Icon = SystemIcons.Application,
			Visible = true,
		};
		World = new(foreground);
		ShutdownMode = ShutdownMode.OnExplicitShutdown;
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		var menu = new ContextMenuStrip();
		OnMenuCreating(menu);

		TrayIcon.ContextMenuStrip = menu;
		TrayIcon.DoubleClick += ManageClicked;

		World.StartThread();
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
			World.StopThread();
			TrayIcon.Dispose();
		}
		finally
		{
			Shutdown();
		}
	}
}
