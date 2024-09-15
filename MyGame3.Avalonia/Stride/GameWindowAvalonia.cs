using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Stride.Core.Mathematics;
using Stride.Games;
using Stride.Graphics;
using System;
using Point = Stride.Core.Mathematics.Point;

namespace MyGame3.Avalonia.Stride;
public class GameWindowAvalonia : GameWindow<Control>
{

	public override bool Visible
	{
		get => (control.GetVisualRoot() as Window)?.IsVisible ?? false;
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
					window.Cursor = isMouseVisible ? new Cursor(StandardCursorType.Arrow) : new Cursor(StandardCursorType.None);
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
	public override DisplayOrientation CurrentOrientation { get; }
	public override bool IsMinimized { get; }
	public override bool Focused { get; }
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

	public GameWindowAvalonia(Control control)
	{
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

	public override void BeginScreenDeviceChange(bool willBeFullScreen)
	{
		deviceChangeWillBeFullScreen = willBeFullScreen;
	}

	public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
	{
		if (!deviceChangeWillBeFullScreen.HasValue)
			return;

		isFullScreenMaximized = deviceChangeWillBeFullScreen.Value;

		if (control != null)
		{
			if (deviceChangeWillBeFullScreen.Value)
			{
				// Switch to full-screen mode
				if (control.GetVisualRoot() is Window window)
				{
					window.WindowState = WindowState.FullScreen;
				}
			}
			else
			{
				// Switch to windowed mode
				if (control.GetVisualRoot() is Window window)
				{
					window.WindowState = WindowState.Normal;
					window.Width = clientWidth;
					window.Height = clientHeight;
				}
			}
		}

		deviceChangeWillBeFullScreen = null;
	}

	protected override void Initialize(GameContext<Control> gameContext)
	{
		control = gameContext.Control;

		// Get the native window handle
		var nativeHandle = GetNativeWindowHandle(control);
		windowHandle = new WindowHandle(AppContextType.Desktop, control, nativeHandle);

		// Subscribe to control events
		control.AttachedToVisualTree += OnAttachedToVisualTree;
		control.DetachedFromVisualTree += OnDetachedFromVisualTree;
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
		//if (e.Property == control.BoundsProperty)
		//{
		//	OnClientSizeChanged();
		//}
	}

	private void StartRendering()
	{
		renderTimer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMilliseconds(16) // Approx. 60 FPS
		};
		renderTimer.Tick += (s, e) => Render();
		renderTimer.Start();
	}

	private void StopRendering()
	{
		renderTimer?.Stop();
		renderTimer = null;
		Destroy();
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

		// Dispose of graphics resources if needed
		ExitCallback?.Invoke();
	}

	public override void Resize(int width, int height)
	{
		if (control.GetVisualRoot() is Window window)
		{
			window.Width = width;
			window.Height = height;
		}
	}

	public override void SetSupportedOrientations(DisplayOrientation orientations)
	{

	}

	protected override void SetTitle(string title)
	{
		var test = control.GetVisualRoot();
	}
}
