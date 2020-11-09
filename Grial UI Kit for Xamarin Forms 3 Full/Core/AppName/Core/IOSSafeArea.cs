using System;

namespace AppName.Core
{
	[Flags]
	public enum IOSSafeArea : byte
	{
		None = 0x0,
		Top = 0x1,
		Right = 0x2,
		Bottom = 0x4,
		Left = 0x8,
		All = 0xF
	}
}
