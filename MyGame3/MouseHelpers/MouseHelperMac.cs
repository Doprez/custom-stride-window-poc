using Stride.Core.Mathematics;
using System.Runtime.InteropServices;

namespace MyGame3.MouseHelpers;
public static class MouseHelperMac
{
	[DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern void CGWarpMouseCursorPosition(CGPoint newCursorPosition);

	[DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
	private static extern CGPoint CGEventSourceStateCombinedSessionState(int sourceStateID);

	[StructLayout(LayoutKind.Sequential)]
	private struct CGPoint
	{
		public double X;
		public double Y;
	}

	public static void SetCursorPosition(int x, int y)
	{
		CGPoint point = new CGPoint { X = x, Y = y };
		CGWarpMouseCursorPosition(point);
	}

	public static Point GetCursorPosition()
	{
		CGPoint point = CGEventSourceStateCombinedSessionState(0);
		return new Point((int)point.X, (int)point.Y);
	}
}
