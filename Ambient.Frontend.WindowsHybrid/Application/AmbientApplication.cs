using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Ambient.Backend.Kernel;
using Ambient.Frontend.WindowsHybrid.Graphics;
using Ambient.Frontend.WindowsHybrid.Utilities;

namespace Ambient.Frontend.WindowsHybrid.Application;

public class AmbientApplication : System.Windows.Application
{
	public NotifyIcon TrayIcon { get; }

	public World World { get; }

	public VirtualViewport Viewport { get; }

	public FormFactory? FormFactory { get; set; }

	public string Name
	{
		get => TrayIcon.Text;
		set
		{
			TrayIcon.Text = value;
			Viewport.Title = value;
		}
	}

	public Icon? Icon
	{
		get => TrayIcon.Icon;
		set => TrayIcon.Icon = value;
	}

	public AmbientApplication()
	{
		var foreground = new DispatcherSynchronizationContext(Dispatcher);
		var world = new World(foreground);

		var bounds = ScreenInformation.GetCombinedWorkingArea();

		TrayIcon = new()
		{
			Text = "Ambient Application",
			Icon = SystemFunctions.ExtractApplicationIcon(),
			Visible = true,
		};
		World = world;
		Viewport = new(world, bounds);

		ShutdownMode = ShutdownMode.OnExplicitShutdown;
		FormFactory = null;
	}

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

	public void Manage()
	{
		FormFactory?.ShowForm();
	}

	public new void Exit()
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

	private void OnManageClicked(object? sender, EventArgs e) => Manage();

	private void OnExitClicked(object? sender, EventArgs e) => Exit();
}
