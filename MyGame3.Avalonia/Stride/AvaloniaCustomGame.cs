using Avalonia.Controls;
using MyGame3.Avalonia.Input;
using Stride.Games;
using Stride.Graphics;
using Stride.Input;
using System;

namespace MyGame3.Avalonia.Stride;
public class AvaloniaCustomGame : GameCopyTest
{
	private Control _control;

	public AvaloniaCustomGame(GamePlatform gamePlatform, Control control) : base(gamePlatform)
	{
		_control = control;
	}

	protected override void Initialize()
	{
		base.Initialize();

		// Add the Avalonia input source
		//var input = Services.GetService<InputManager>();
		//var avaloniaInput = new InputSourceAvalonia(_control);
		//input.Sources.Add(avaloniaInput);
	}

	protected override void BeginRun()
	{
		MinimizedMinimumUpdateRate.MinimumElapsedTime = TimeSpan.FromMilliseconds(0);
		WindowMinimumUpdateRate.MinimumElapsedTime = TimeSpan.FromMilliseconds(0);
		GraphicsDevice.Presenter.PresentInterval = PresentInterval.Immediate;
	}
}
