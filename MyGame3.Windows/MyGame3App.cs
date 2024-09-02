using MyGame3;
using MyGame3.SDL;
using MyGame3.SDL.CustomWindowing;

using var game = new CustomGame();

var window = new GameWindowSDL("MyGame Custom Window");

game.Run(window.GameContext, window);

