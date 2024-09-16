using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Silk.NET.SDL;
using Stride.Input;
using static Stride.Input.VirtualButton;

namespace MyGame3.Avalonia.Input;

/// <summary>
/// Provides support for keyboard input using Avalonia.
/// </summary>
internal class InputSourceAvalonia : InputSourceBase
{
	private readonly Control _uiControl;
	private InputManager _inputManager;

	private KeyboardAvalonia _keyboard;
	private MouseAvalonia _mouse;

	private Sdl _sdl;

	public InputSourceAvalonia(Control uiControl)
	{
		this._uiControl = uiControl ?? throw new ArgumentNullException(nameof(uiControl));

		_sdl = Sdl.GetApi();
		_sdl.Init(Sdl.InitGamecontroller);
	}

	public override void Initialize(InputManager inputManager)
	{
		this._inputManager = inputManager;

		// Create the keyboard device
		_keyboard = new KeyboardAvalonia(this, _uiControl);
		RegisterDevice(_keyboard);

		// Create and register the mouse device
		_mouse = new MouseAvalonia(this, _uiControl, _sdl);
		RegisterDevice(_mouse);

		// Avalonia does not have joystick device added/removed events like SDL
		// So for gamepads, we may need to handle them differently in the future
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


