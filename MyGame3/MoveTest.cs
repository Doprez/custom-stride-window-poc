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
		// Do stuff every new frame
		Entity.Transform.Position.X += 0.01f;
		Physics.UpdatePhysicsTransformation();

		if(Input.IsKeyPressed(Stride.Input.Keys.Tab))
		{
			GraphicsDevice.Presenter.IsFullScreen = true;
			//Game.Window.Visible = false;
			//Game.Window.IsFullscreen = true;
			//Game.Window.SetBorderlessWindowFullScreen(!Game.Window.FullscreenIsBorderlessWindow);
		}
	}
}
