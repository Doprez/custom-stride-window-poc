using Avalonia.Controls;
using MyGame3.Avalonia.Input;
using Stride.Games;
using Stride.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		var input = Services.GetService<InputManager>();
		var avaloniaInput = new InputSourceAvalonia(_control);
		input.Sources.Add(avaloniaInput);
	}

	protected override void BeginRun()
	{
		base.BeginRun();
		// TODO: This is a temporary workaround to avoid low FPS until I manage window focus events.
		MinimizedMinimumUpdateRate.MinimumElapsedTime = TimeSpan.FromMilliseconds(1000f / 120);
	}
}
