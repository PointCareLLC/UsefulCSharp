﻿/*
Useful C#
Copyright (C) 2014  Nicholas Randal

Useful C# is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Randal.Utilities.Sql.Deployer.Configuration;

namespace Randal.Utilities.Sql.Deployer.Scripts
{
	public interface IProject
	{
		IProjectConfig Configuration { get; }
		IReadOnlyList<SourceScript> NonPriorityScripts { get; }
		IReadOnlyList<SourceScript> PriorityScripts { get; }

		SourceScript TryGetScript(string scriptName);
	}

	public sealed class Project : IProject
	{
		public Project(IProjectConfig config, IEnumerable<SourceScript> scripts)
		{
			if(config == null)
				throw new ArgumentNullException("config");
			if(scripts == null)
				throw new ArgumentNullException("scripts");

			var allScripts = scripts.ToList();

			Configuration = config;
			_nonPriorityScripts = allScripts.ToList();
			_allScriptsLookup = allScripts.ToDictionary(s => s.Name.ToLower(), s => s, StringComparer.InvariantCultureIgnoreCase);
			_priorityScripts = new List<SourceScript>();
			SetupPriorityScripts();

			_nonPriorityScripts.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
		}

		public IProjectConfig Configuration { get; private set; }
		public IReadOnlyList<SourceScript> NonPriorityScripts { get { return _nonPriorityScripts.AsReadOnly(); } }
		public IReadOnlyList<SourceScript> PriorityScripts { get { return _priorityScripts; } }

		public SourceScript TryGetScript(string scriptName)
		{
			SourceScript script;
			_allScriptsLookup.TryGetValue(scriptName, out script);
			return script;
		}

		private void SetupPriorityScripts()
		{
			foreach (var priorityName in Configuration.PriorityScripts)
			{
				var script = TryGetScript(priorityName);

				if(script == null)
					throw new InvalidOperationException("Script '" + priorityName + "' listed in the configuration as a priority script was not found.");

				_priorityScripts.Add(script);
				_nonPriorityScripts.Remove(script);
			}
		}

		private readonly Dictionary<string, SourceScript> _allScriptsLookup;
		private readonly List<SourceScript> _priorityScripts;
		private readonly List<SourceScript> _nonPriorityScripts;
	}
}