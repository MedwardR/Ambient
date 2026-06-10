using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Ambient.Backend.Kernel;

namespace Ambient.Frontend.WindowsHybrid.Application;

/// <summary>
/// Encapsulates a WinForms/WPF application utilizing the Ambient engine.
/// </summary>
public class AmbientApplication : System.Windows.Application
{
	/// <summary>
	/// An event that is raised when the user requests to 'manage' the application
	/// (for example, by pressing the 'Manage' button in the system tray).
	/// </summary>
	public event EventHandler? ManageRequested;

	/// <summary>
	/// The name of the application (used in the system tray).
	/// </summary>
	public string Name
	{
		get => TrayIcon.Text;
		set => TrayIcon.Text = value;
	}

	/// <summary>
	/// A Windows system tray icon representing the app and
	/// providing a simple user interface.
	/// </summary>
	public NotifyIcon TrayIcon { get; }

	/// <inheritdoc cref="Backend.Kernel.World"/>
	public World World { get; }

	/// <inheritdoc cref="AmbientApplication"/>
	public AmbientApplication()
	{
		TrayIcon = new()
		{
			Text = "Ambient Application",
			Icon = SystemIcons.Application,
			Visible = true,
		};
		World = new();
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
			World.StopThread();
		}
		finally
		{
			base.OnExit(e);
		}
	}
}
