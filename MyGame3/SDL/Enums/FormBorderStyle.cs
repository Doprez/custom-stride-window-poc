﻿namespace MyGame3.SDL.Enums;
/// <summary>
/// Set of border style to mimic the Windows forms one. We actually only show <see cref="FixedSingle"/> and <see cref="Sizable"/> as the
/// other values don't make sense in a purely SDL context.
/// </summary>
public enum FormBorderStyle
{
	/// <summary>
	/// Borderless
	/// </summary>
	None = 0,

	/// <summary>
	/// Borders but not resizeable
	/// </summary>
	FixedSingle = 1,

	/// <summary>
	/// Borders and resizeable
	/// </summary>
	Sizable = 4,
}
