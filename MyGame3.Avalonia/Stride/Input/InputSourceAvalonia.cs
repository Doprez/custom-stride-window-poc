using System;
using Avalonia.Controls;
using Stride.Input;

namespace MyGame3.Avalonia.Input;

/// <summary>
/// Provides support for keyboard input using Avalonia. must be run on the Avalonia UI thread.
/// </summary>
internal class InputSourceAvalonia : InputSourceBase
{
	private readonly Control _uiControl;
	private InputManager _inputManager;

	private KeyboardAvalonia _keyboard;
	private MouseAvalonia _mouse;

	public InputSourceAvalonia(Control uiControl)
	{
		_uiControl = uiControl ?? throw new ArgumentNullException(nameof(uiControl));
	}

	public override void Initialize(InputManager inputManager)
	{
		_inputManager = inputManager;

		// Create the keyboard device
		_keyboard = new KeyboardAvalonia(this, _uiControl);
		RegisterDevice(_keyboard);

		// Create and register the mouse device
		_mouse = new MouseAvalonia(this, _uiControl);
		RegisterDevice(_mouse);
	}

	public override void Dispose()
	{
		// Dispose of the keyboard device
		_keyboard.Dispose();
		_mouse?.Dispose();

		base.Dispose();
	}

	public override void Update()
	{
		// Update devices if necessary
		// KeyboardAvalonia updates are handled during the input manager's update cycle
	}

	public override void Scan()
	{
		// No scanning required for keyboard
		// Implement gamepad scanning here in the future if needed
	}
}


