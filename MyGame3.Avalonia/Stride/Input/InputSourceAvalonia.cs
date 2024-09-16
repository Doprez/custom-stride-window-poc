using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Stride.Input;
using static Stride.Input.VirtualButton;

namespace MyGame3.Avalonia.Input;

/// <summary>
/// Provides support for keyboard input using Avalonia.
/// </summary>
internal class InputSourceAvalonia : InputSourceBase
{
	private readonly Control uiControl;
	private InputManager inputManager;

	private KeyboardAvalonia keyboard;
	private MouseAvalonia mouse;

	public InputSourceAvalonia(Control uiControl)
	{
		this.uiControl = uiControl ?? throw new ArgumentNullException(nameof(uiControl));
	}

	public override void Initialize(InputManager inputManager)
	{
		this.inputManager = inputManager;

		// Create the keyboard device
		keyboard = new KeyboardAvalonia(this, uiControl);
		RegisterDevice(keyboard);

		// Create and register the mouse device
		mouse = new MouseAvalonia(this, uiControl);
		RegisterDevice(mouse);

		// Avalonia does not have joystick device added/removed events like SDL
		// So for gamepads, we may need to handle them differently in the future
	}

	public override void Dispose()
	{
		// Dispose of the keyboard device
		keyboard.Dispose();
		mouse?.Dispose();

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


