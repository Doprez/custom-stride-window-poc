using Avalonia.Controls;
using Avalonia.Interactivity;
using MyGame3.Avalonia.Input;
using MyGame3.Avalonia.Stride;
using Stride.Engine;
using Stride.Games;
using Stride.Input;
using System;
using System.Threading.Tasks;

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

		game.Run(gameWindow.GameContext);
	}
}