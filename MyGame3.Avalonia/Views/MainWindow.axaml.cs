using Avalonia.Controls;
using Avalonia.Interactivity;
using MyGame3.Avalonia.Stride;
using Stride.Engine;
using Stride.Games;
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
		game = new GameCopyTest(gamePlatform);

		game.Run(gameWindow.GameContext);
	}
}