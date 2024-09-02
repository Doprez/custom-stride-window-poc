using Stride.Input;
using System;

namespace MyGame3.SDL.Input;

/// <summary>
/// A known gamepad that uses SDL as a driver
/// </summary>
internal class GamePadSDL : GamePadFromLayout, IDisposable, IGamePadDevice
{
	private bool disposed;

	public GamePadSDL(InputSourceSDL source, InputManager inputManager, GameControllerSDL controller, GamePadLayout layout)
		: base(inputManager, controller, layout)
	{
		Source = source;
		Name = controller.Name;
		Id = controller.Id;
		ProductId = controller.ProductId;
	}

	public void Dispose()
	{
		if (!disposed)
		{
			// ReSharper disable once PossibleNullReferenceException (checked in constructor)
			(GameControllerDevice as GameControllerSDL).Dispose();

			disposed = true;
		}
	}

	public override string Name { get; }
	public override Guid Id { get; }
	public override Guid ProductId { get; }
	public override IInputSource Source { get; }

	public new int Index
	{
		get { return base.Index; }
		set { SetIndexInternal(value, false); }
	}

	public override void SetVibration(float smallLeft, float smallRight, float largeLeft, float largeRight)
	{
		// No vibration support in SDL gamepads
	}
}
