using MyGame3.SDL;
using MyGame3.SDL.Input;
using Stride.Engine;

namespace MyGame3;
public class CustomGame : Game
{
	protected override void Initialize()
	{
		base.Initialize();
		var sdlContext = (GameContextSDL)Context;
		Input.Sources.Add(new InputSourceSDL(sdlContext.Control));
	}
}
