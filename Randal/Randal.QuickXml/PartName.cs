﻿// // Useful C#
// // Copyright (C) 2014 Nicholas Randal
// // 
// // Useful C# is free software; you can redistribute it and/or modify
// // it under the terms of the GNU General Public License as published by
// // the Free Software Foundation; either version 2 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// // GNU General Public License for more details.

using System;

namespace Randal.QuickXml
{
	public sealed class PartName
	{
		public PartName(string name)
		{
			var parts = name.Split(Splitter, 2, StringSplitOptions.RemoveEmptyEntries);
			IsTwoPart = parts.Length == 2;
			One = parts[0].Trim();

			if (IsTwoPart)
				Two = parts[1].Trim();
		}

		public bool IsTwoPart { get; private set; }

		public string One { get; private set; }
		public string Two { get; private set; }

		private static readonly char[] Splitter = {':'};
	}
}