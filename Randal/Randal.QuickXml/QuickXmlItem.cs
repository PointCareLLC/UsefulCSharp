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

using System.Xml;

namespace Randal.QuickXml
{
	public interface IQuickXmlItem
	{
		XmlNodeType Type { get; }
		int Depth { get; }
		string Name { get; }
		string Value { get; }
	}

	public abstract class QuickXmlItem : IQuickXmlItem
	{
		protected QuickXmlItem(XmlNodeType type, int depth)
		{
			Type = type;
			Depth = depth;
		}

		public XmlNodeType Type { get; protected set; }
		public int Depth { get; protected set; }
		public virtual string Name { get; protected set; }
		public virtual string Value { get; protected set; }
	}
}