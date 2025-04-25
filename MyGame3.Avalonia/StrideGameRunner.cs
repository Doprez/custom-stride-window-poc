using Stride.Engine;
using Stride.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyGame3.Avalonia;
public class StrideGameRunner
{
	private Thread _gameThread;
	private bool _isRunning;
	private GameBase _gameInstance;
	private GameContext _gameContext;

	public StrideGameRunner(GameBase game, GameContext context)
	{
		_gameContext = context;
		_gameInstance = game;
		_isRunning = true;
		_gameThread = new Thread(GameLoop)
		{
			IsBackground = true,
			Name = "StrideGameThread"
		};
		_gameThread.Start();
	}

	private void GameLoop()
	{
		// Update and render the game
		_gameInstance.Run();
	}

	public void Stop()
	{
		_isRunning = false;
		_gameThread.Join();
	}
}
