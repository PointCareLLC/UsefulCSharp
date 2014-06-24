﻿// Useful C#
// Copyright (C) 2014 Nicholas Randal
// 
// Useful C# is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

using Newtonsoft.Json;

namespace Randal.Utilities.Sql.Deployer.Scripts
{
	public sealed class ScriptSettings
	{
		public ScriptSettings() : this(30)
		{
		}

		public ScriptSettings(int timeout)
		{
			Timeout = timeout;
		}

		[JsonProperty(Required = Required.Default)]
		public int Timeout { get; private set; }
	}
}