using Stride.Engine;
using Stride.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalGame.Windows;
public class CustomGame : DefaultGame
{

    protected override void BeginRun()
    {
        MinimizedMinimumUpdateRate.MinimumElapsedTime = TimeSpan.FromMilliseconds(0);
        WindowMinimumUpdateRate.MinimumElapsedTime = TimeSpan.FromMilliseconds(0);
        GraphicsDevice.Presenter.PresentInterval = PresentInterval.Immediate;
    }

}
