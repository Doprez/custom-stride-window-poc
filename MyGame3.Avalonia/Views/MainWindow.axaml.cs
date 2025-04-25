using Avalonia.Controls;
using Avalonia.Interactivity;
using MyGame3.Avalonia.Stride;
using Stride.Core;
using Stride.Games;
using System;

namespace MyGame3.Avalonia.Views;
public partial class MainWindow : Window
{
	private GamePlatformAvalonia gamePlatform;
	private GameContextAvalonia gameContext;
	private GameBase game;

	public MainWindow()
	{
		InitializeComponent();

		Loaded += OnLoaded;
		Closed += OnClosed;
	}

	private void OnLoaded(object? sender, RoutedEventArgs e)
	{
		gameContext = new GameContextAvalonia(this);
		gamePlatform = new GamePlatformAvalonia(new ServiceRegistry(), gameContext);
		game = new AvaloniaCustomGame(gamePlatform, gameContext.Control);

		game.Run();
	}

	private void OnClosed(object? sender, EventArgs e)
	{
		game.Exit();
	}
}