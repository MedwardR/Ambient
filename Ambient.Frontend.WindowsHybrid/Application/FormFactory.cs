using System;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Ambient.Frontend.WindowsHybrid.Application;

public class FormFactory : IDisposable
{
	protected readonly Func<Form> _factory;
	protected readonly DispatcherTimer _recycleTimer;

	public Form? FormInstance { get; protected set; }

	public TimeSpan RecycleTimeout { get; set; }

	public FormFactory(Func<Form> factory)
	{
		_factory = factory;
		_recycleTimer = new();

		FormInstance = null;
		RecycleTimeout = TimeSpan.FromMinutes(5);

		_recycleTimer.Tick += OnRecycle;
	}

	public virtual void ShowForm()
	{
		_recycleTimer.Stop();

		if (FormInstance is null || FormInstance.IsDisposed)
		{
			FormInstance = _factory();
			FormInstance.FormClosing += OnClosing;
		}
		FormInstance.Show();
		FormInstance.Activate();
		FormInstance.Focus();
	}

	protected virtual void OnClosing(object? sender, FormClosingEventArgs e)
	{
		if (FormInstance is not null && e.CloseReason == CloseReason.UserClosing)
		{
			FormInstance.Hide();

			_recycleTimer.Interval = RecycleTimeout;

			_recycleTimer.Stop();
			_recycleTimer.Start();

			e.Cancel = true;
		}
	}

	protected virtual void OnRecycle(object? sender, EventArgs e)
	{
		_recycleTimer.Stop();

		if (FormInstance is not null && !FormInstance.Visible)
		{
			if (!FormInstance.IsDisposed)
			{
				FormInstance.Dispose();
			}
			FormInstance = null;
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
			_recycleTimer.Stop();

			if (FormInstance is not null && !FormInstance.IsDisposed)
			{
				FormInstance.Dispose();
			}
			FormInstance = null;
		}
	}
}
