using Avalonia.Controls;
using Avalonia.Interactivity;
using MyGame3.Avalonia.Input;
using MyGame3.Avalonia.Stride;
using Stride.Engine;
using Stride.Games;
using Stride.Input;
using System;

namespace MyGame3.Avalonia.Views;
public partial class MainWindow : Window
{
	private GamePlatformAvalonia gamePlatform;
	private GameWindow<Control> gameWindow;
	private GameBase game;

	public MainWindow()
	{
		InitializeComponent();

		Loaded += OnLoaded;
	}

	private void OnLoaded(object? sender, RoutedEventArgs e)
	{
		gameWindow = new GameWindowAvalonia(this);
		gamePlatform = new GamePlatformAvalonia();
		game = new AvaloniaCustomGame(gamePlatform, gameWindow.GameContext.Control);

		//var input = game.Services.GetService<InputManager>();
		//// I could just use "this" as the control but I want to be clear that I am using
		//// the control from the GameContext.
		//var avaloniaInput = new InputSourceAvalonia(gameWindow.GameContext.Control);
		//input.Sources.Add(avaloniaInput);

		game.Run(gameWindow.GameContext);
	}
}