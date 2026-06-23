using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Ambient.Backend.Kernel;

namespace Ambient.Frontend.WindowsHybrid.Application;

public class AmbientApplication : System.Windows.Application
{
	public string Name
	{
		get => TrayIcon.Text;
		set => TrayIcon.Text = value;
	}

	public Icon? Icon
	{
		get => TrayIcon.Icon;
		set => TrayIcon.Icon = value;
	}

	public NotifyIcon TrayIcon { get; }

	public World World { get; }

	public FormFactory? FormFactory { get; set; }

	public AmbientApplication()
	{
		var foreground = new DispatcherSynchronizationContext(Dispatcher);

		TrayIcon = new()
		{
			Text = "Ambient Application",
			Icon = GetIconOrDefault(),
			Visible = true,
		};
		World = new(foreground);
		ShutdownMode = ShutdownMode.OnExplicitShutdown;
		FormFactory = null;
	}

	public void Manage() => OnManageRequested();

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		var menu = new ContextMenuStrip();
		OnMenuCreating(menu);

		TrayIcon.ContextMenuStrip = menu;
		TrayIcon.DoubleClick += OnManageClicked;

		World.StartThread();
	}

	protected virtual void OnMenuCreating(ContextMenuStrip menu)
	{
		menu.Items.Add("Manage", null, OnManageClicked);
		menu.Items.Add("Exit", null, OnExitClicked);
	}

	protected virtual void OnManageRequested()
	{
		FormFactory?.ShowForm();
	}

	private static Icon GetIconOrDefault()
	{
		var path = Environment.ProcessPath;

		if (File.Exists(path))
		{
			var ico = Icon.ExtractAssociatedIcon(path);
			return ico ?? SystemIcons.Application;
		}
		else return SystemIcons.Application;
	}

	private void OnManageClicked(object? sender, EventArgs e)
	{
		OnManageRequested();
	}

	private void OnExitClicked(object? sender, EventArgs e)
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
