using Stride.Core.Mathematics;
using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame3;
public class MoveTest : SyncScript
{
	public PhysicsComponent Physics;

	public override void Start()
	{
		// Initialization of the script.
		Physics = Entity.Get<PhysicsComponent>();
	}

	public override void Update()
	{
		DebugText.Print($"FPS: {Game.UpdateTime.FramePerSecond}", new Int2(10, 10));

		// Do stuff every new frame
		Entity.Transform.Position.X += 1f * (float)Game.UpdateTime.Elapsed.TotalSeconds;
		Physics.UpdatePhysicsTransformation();

		if(Input.IsKeyPressed(Stride.Input.Keys.Tab))
		{
			Game.Window.SetBorderlessWindowFullScreen(!Game.Window.FullscreenIsBorderlessWindow);
		}
	}
}
