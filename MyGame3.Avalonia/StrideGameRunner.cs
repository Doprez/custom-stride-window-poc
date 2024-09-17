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
	private Thread gameThread;
	private bool isRunning;
	private GameBase gameInstance;
	private GameContext gameContext;

	public StrideGameRunner(GameBase game, GameContext context)
	{
		gameContext = context;
		gameInstance = game;
		isRunning = true;
		gameThread = new Thread(GameLoop)
		{
			IsBackground = true,
			Name = "StrideGameThread"
		};
		gameThread.Start();
	}

	private void GameLoop()
	{
		// Update and render the game
		gameInstance.Run(gameContext);
	}

	public void Stop()
	{
		isRunning = false;
		gameThread.Join();
	}
}
