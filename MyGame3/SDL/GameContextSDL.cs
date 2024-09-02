using SharpDX.Direct3D11;
using Stride.Games;
using GameFormSDL = MyGame3.SDL.CustomWindowing.GameFormSDL;

namespace MyGame3.SDL;

/// <summary>
/// A <see cref="GameContext"/> to use for rendering to an existing SDL Window.
/// </summary>
public class GameContextSDL : GameContext<SDLWindow>
{
	/// <inheritDoc/>
	public GameContextSDL(SDLWindow control, int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false)
		: base(control ?? new GameFormSDL(), requestedWidth, requestedHeight, isUserManagingRun)
	{
		ContextType = AppContextType.Custom;
	}
}
