using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Stride.Core.Mathematics;
using Stride.Games;
using Stride.Graphics;
using System;
using System.Threading;
using Point = Stride.Core.Mathematics.Point;

namespace MyGame3.Avalonia.Stride;
public class GameWindowAvalonia : GameWindow<Control>
{

	public bool IsRunningOnSeparateThread { get; private set; }

	public override bool Visible
	{
		get
		{
			if (IsRunningOnSeparateThread)
			{
				return Dispatcher.UIThread.Invoke(() =>
				{
					return (control.GetVisualRoot() as Window)?.IsVisible ?? false;
				});
			}
			return (control.GetVisualRoot() as Window)?.IsVisible ?? false;
		}
		set
		{
			if (control.GetVisualRoot() is Window window)
			{
				if (value)
					window.Show();
				else
					window.Hide();
			}
		}
	}

	public override Int2 Position
	{
		get
		{
			if (control.GetVisualRoot() is Window window)
			{
				return new Int2((int)window.Position.X, (int)window.Position.Y);
			}
			return base.Position;
		}
		set
		{
			if (control.GetVisualRoot() is Window window)
			{
				window.Position = new PixelPoint(value.X, value.Y);
			}
		}
	}

	public override bool IsMouseVisible
	{
		get => isMouseVisible;
		set
		{
			if (isMouseVisible != value)
			{
				isMouseVisible = value;
				if (control.GetVisualRoot() is Window window)
				{
					Dispatcher.UIThread.Post(() =>
					{
						window.Cursor = isMouseVisible ? new Cursor(StandardCursorType.Arrow) : new Cursor(StandardCursorType.None);
					});
				}
			}
		}
	}

	public override bool AllowUserResizing
	{
		get => allowUserResizing;
		set
		{
			allowUserResizing = value;
			if (control.GetVisualRoot() is Window window)
			{
				window.CanResize = allowUserResizing;
			}
		}
	}

	public override bool IsBorderLess
	{
		get => isBorderLess;
		set
		{
			if (isBorderLess != value)
			{
				isBorderLess = value;
				if (control.GetVisualRoot() is Window window)
				{
					window.SystemDecorations = isBorderLess ? SystemDecorations.None : SystemDecorations.Full;
				}
			}
		}
	}

	public override Rectangle ClientBounds { get => new Rectangle(0, 0, (int)control.Bounds.Right, (int)control.Bounds.Bottom); }
	public override DisplayOrientation CurrentOrientation { get => DisplayOrientation.Default; }
	public override bool IsMinimized
	{
		get
		{
			if (control.GetVisualRoot() is Window window)
			{
				if (IsRunningOnSeparateThread)
				{
					return Dispatcher.UIThread.Invoke(() =>
					{
						return window.WindowState.Equals(WindowState.FullScreen);
					});
				}
				return window.WindowState.Equals(WindowState.Minimized);
			}
			return false;
		}
	}

	public override bool Focused
	{ 
		get
		{
			if (control.GetVisualRoot() is Window window)
			{
				if (IsRunningOnSeparateThread)
				{
					return Dispatcher.UIThread.Invoke(() =>
					{
						return window.IsActive;
					});
				}
				return window.IsActive;
			}
			return true;
		} 
	}
	public override WindowHandle NativeWindow { get => windowHandle; }

	private Control control;
	private WindowHandle windowHandle;
	private bool isMouseVisible;
	private bool isMouseCurrentlyHidden;
	private bool isFullScreenMaximized;
	private Point savedFormLocation;
	private bool? deviceChangeWillBeFullScreen;
	private bool allowUserResizing;
	private bool isBorderLess;

	private DispatcherTimer renderTimer;

	public GameWindowAvalonia(Control control, bool shouldRunInSeparateThread = true)
	{
		IsRunningOnSeparateThread = shouldRunInSeparateThread;
		GameContext = new GameContextAvalonia(control);
		Initialize(GameContext);
	}

	public override void Run()
	{
		InitCallback?.Invoke();

		// Start the rendering loop
		StartRendering();

		// Since Avalonia applications have their own application loop,
		// we don't need to implement a custom message loop here.
	}

	private WindowState savedWindowState;
	private Size lastClientSize;
	private PixelPoint lastClientPos;

	public override void SetBorderlessWindowFullScreen(bool borderlessFullScreen)
	{
		FullscreenIsBorderlessWindow = borderlessFullScreen;
		if (borderlessFullScreen)
		{
			if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				var size = desktop.MainWindow.Screens.Primary.Bounds.Size;
				if (control.GetVisualRoot() is Window window)
				{
					lastClientPos = window.Position;
					window.SystemDecorations = SystemDecorations.None;
					savedWindowState = window.WindowState;
					window.WindowState = WindowState.FullScreen;
					window.Position = new PixelPoint(0, 0);
					lastClientSize = window.ClientSize;
					SetSize(new Int2(size.Width, size.Height));
					window.Topmost = true;
				}
			}
		}
		else
		{
			if (control.GetVisualRoot() is Window window)
			{
				window.SystemDecorations = SystemDecorations.Full;
				window.Position = lastClientPos;
				window.Width = lastClientSize.Width;
				window.Height = lastClientSize.Height;
				window.WindowState = savedWindowState;
			}
		}
	}

	private SystemDecorations savedFormBorderStyle;
	private bool oldVisible;
	private bool deviceChangeChangedVisible;

	public override void BeginScreenDeviceChange(bool willBeFullScreen)
	{
		if (control.GetVisualRoot() is Window window)
		{
			if (willBeFullScreen && !isFullScreenMaximized)
			{
				savedFormBorderStyle = window.SystemDecorations;
			}

			if (willBeFullScreen != isFullScreenMaximized)
			{
				deviceChangeChangedVisible = true;
				oldVisible = Visible;
				Visible = false;
				window.Topmost = false;
			}
			else
			{
				deviceChangeChangedVisible = false;
			}

			if (!willBeFullScreen && isFullScreenMaximized)
			{
				window.Topmost = false;
				window.SystemDecorations = savedFormBorderStyle;
			}

			deviceChangeWillBeFullScreen = willBeFullScreen;
		}
	}

	public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
	{
		if (!deviceChangeWillBeFullScreen.HasValue)
			return;

		if (control.GetVisualRoot() is Window window)
		{
			if (deviceChangeWillBeFullScreen.Value)
			{
				isFullScreenMaximized = true;
			}
			else if (isFullScreenMaximized)
			{
				window.Topmost = true;
				isFullScreenMaximized = false;
			}

			//UpdateFormBorder();

			if (deviceChangeChangedVisible)
				Visible = oldVisible;

			//window.ClientSize = new Size(clientWidth, clientHeight);

			deviceChangeWillBeFullScreen = null;
		}
	}

	protected override void Initialize(GameContext<Control> gameContext)
	{
		control = gameContext.Control;

		// Get the native window handle
		var nativeHandle = GetNativeWindowHandle(control);
		windowHandle = new WindowHandle(AppContextType.Desktop, control, nativeHandle);

		// Subscribe to control events
		//control.AttachedToVisualTree += OnAttachedToVisualTree;
		//control.DetachedFromVisualTree += OnDetachedFromVisualTree;
		control.PropertyChanged += OnPropertyChanged;
	}

	private void OnAttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
	{
		// Start rendering when the control is attached to the visual tree
		StartRendering();
	}

	private void OnDetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e)
	{
		// Stop rendering and clean up when the control is detached
		StopRendering();
	}

	private void OnPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
	{
		if (e.Property == Control.BoundsProperty)
		{
			OnClientSizeChanged(control, null);
		}
	}

	private Thread renderThread;
	private bool isRunning;

	private void StartRendering()
	{
		if (IsRunningOnSeparateThread)
		{
			isRunning = true;
			renderThread = new Thread(() =>
			{
				// Initialize a stopwatch to control frame rate
				var stopwatch = new System.Diagnostics.Stopwatch();
				const double targetFrameTime = 0;

				while (isRunning)
				{
					stopwatch.Restart();

					Render();

					stopwatch.Stop();
					var elapsed = stopwatch.Elapsed.TotalMilliseconds;
					var sleepTime = targetFrameTime - elapsed;

					if (sleepTime > 0)
					{
						Thread.Sleep((int)sleepTime);
					}
				}
			})
			{
				IsBackground = true,
				Name = "StrideRenderThread"
			};
			renderThread.Start();
		}
		else
		{
			renderTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(0)
			};

			renderTimer.Tick += (s, e) => Render();
			renderTimer.Start();
		}
	}

	private void StopRendering()
	{
		renderTimer?.Stop();
		renderTimer = null;
	}

	private void Render()
	{
		if (Exiting)
		{
			Destroy();
			return;
		}

		// Update and draw your Stride3D scene here
		RunCallback();
	}

	private IntPtr GetNativeWindowHandle(Control control)
	{
		if (control.GetVisualRoot() is Window topLevel)
		{
			var platformImpl = topLevel.TryGetPlatformHandle();
			return platformImpl?.Handle ?? IntPtr.Zero;
		}
		return IntPtr.Zero;
	}

	protected override void Destroy()
	{
		StopRendering();

		ExitCallback?.Invoke();
	}

	public override void Resize(int width, int height)
	{
		if (control.GetVisualRoot() is Window window)
		{
			Dispatcher.UIThread.Post(() =>
			{
				window.Width = width;
				window.Height = height;
			});
		}
	}

	public override void SetSupportedOrientations(DisplayOrientation orientations)
	{

	}

	protected override void SetTitle(string title)
	{
		if (control.GetVisualRoot() is Window window)
		{
			window.Title = title;
		}
	}
}
