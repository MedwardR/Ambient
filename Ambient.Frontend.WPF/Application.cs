using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Ambient.Kernel;

namespace Ambient.Frontend.WindowsHybrid;

public class Application : System.Windows.Application
{
	public event EventHandler? ManageRequested;

	public string Name { get; init; } = "Ambient Application";

	public NotifyIcon TrayIcon { get; } = new();
	public World World { get; } = new();

	protected Stopwatch _sw = new();

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		TrayIcon.Icon = SystemIcons.Application;
		TrayIcon.Text = Name;
		TrayIcon.Visible = true;

		var menu = new ContextMenuStrip();
		OnMenuCreating(menu);

		TrayIcon.ContextMenuStrip = menu;
		TrayIcon.DoubleClick += OnManage;
		CompositionTarget.Rendering += OnRendering;
	}

	protected virtual void OnMenuCreating(ContextMenuStrip menu)
	{
		menu.Items.Add("Manage", null, OnManage);
		menu.Items.Add("Exit", null, OnExit);
	}

	private void OnManage(object? sender, EventArgs e)
	{
		ManageRequested?.Invoke(this, EventArgs.Empty);
	}

	private void OnExit(object? sender, EventArgs e)
	{
		TrayIcon.Dispose();
		Shutdown();
	}

	protected virtual void OnRendering(object? sender, EventArgs e)
	{
		float deltaTime = (float)_sw.Elapsed.TotalSeconds;
		_sw.Restart();

		World.Update(deltaTime);
	}
}
