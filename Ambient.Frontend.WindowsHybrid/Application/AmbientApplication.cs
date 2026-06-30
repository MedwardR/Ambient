using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Ambient.Backend.Kernel;
using Ambient.Frontend.WindowsHybrid.Extensions;
using Ambient.Frontend.WindowsHybrid.Graphics;
using Ambient.Frontend.WindowsHybrid.Utilities;

namespace Ambient.Frontend.WindowsHybrid.Application;

public class AmbientApplication : System.Windows.Application
{
	public NotifyIcon TrayIcon { get; }

	public World World { get; }

	public VirtualViewport Viewport { get; }

	public FormFactory? FormFactory { get; set; }

	public AmbientApplication(string name)
	{
		var foreground = new DispatcherSynchronizationContext(Dispatcher);

		var world = new World(foreground);
		var bounds = ScreenInformation.GetCombinedWorkingArea().ToRect();

		TrayIcon = new()
		{
			Text = name,
			Icon = SystemFunctions.ExtractApplicationIcon(),
			Visible = true,
		};
		World = world;
		Viewport = new(world, bounds);

		ShutdownMode = ShutdownMode.OnExplicitShutdown;
		FormFactory = null;

		Viewport.Window.Title = name;

		TrayIcon.Click += OnClick;
		TrayIcon.DoubleClick += OnManageClicked;
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		var menu = new ContextMenuStrip();
		OnMenuCreating(menu);

		TrayIcon.ContextMenuStrip = menu;

		World.StartThread();
	}

	protected virtual void OnMenuCreating(ContextMenuStrip menu)
	{
		menu.Items.Add("Manage", null, OnManageClicked);
		menu.Items.Add("Exit", null, OnExitClicked);
	}

	public void FocusViewport()
	{
		Viewport.Window.Show();
		Viewport.Window.Activate();
		Viewport.Window.Focus();
	}

	public void Manage()
	{
		FormFactory?.ShowForm();
	}

	public void Quit()
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

	private void OnClick(object? sender, EventArgs e) => FocusViewport();

	private void OnManageClicked(object? sender, EventArgs e) => Manage();

	private void OnExitClicked(object? sender, EventArgs e) => Quit();
}
