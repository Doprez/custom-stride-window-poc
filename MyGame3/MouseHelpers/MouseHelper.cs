using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyGame3.MouseHelpers;
public static class MouseHelper
{
	public static void SetCursorPosition(int x, int y)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			MouseHelperWindows.SetCursorPosition(x, y);
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			MouseHelperX11.SetCursorPosition(x, y);
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			MouseHelperMac.SetCursorPosition(x, y);
		}
	}

	public static Point GetCursorPosition()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return MouseHelperWindows.GetCursorPosition();
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			return MouseHelperX11.GetCursorPosition();
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			return MouseHelperMac.GetCursorPosition();
		}
		else
		{
			throw new PlatformNotSupportedException("Getting cursor position is not supported on this platform.");
		}
	}
}
