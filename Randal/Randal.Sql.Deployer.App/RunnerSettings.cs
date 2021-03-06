﻿// Useful C#
// Copyright (C) 2014-2016 Nicholas Randal
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

using System;
using Randal.Logging;

namespace Randal.Sql.Deployer.App
{
	public interface IRunnerSettings
	{
		IRollingFileSettings FileLoggerSettings { get; }
		string ScriptProjectFolder { get; }
		string Server { get; }
        string ConnectionString { get; }
        string DeploymentsDb { get; }
        bool NoTransaction { get; }
		bool UseTransaction { get; }
		bool ShouldRollback { get; }
		bool CheckFilesOnly { get; }
		bool BypassCheck { get; }
        bool ForceInstall { get; }
    }

	public sealed class RunnerSettings : IRunnerSettings
	{
        public RunnerSettings(string scriptProjectFolder, string logFolder, string server, string connectionString, string deploymentsDb,
            bool rollback = false, bool noTransaction = false, bool checkFilesOnly = false, bool bypassCheck = false, bool forceInstall = false)
        {
            if (checkFilesOnly && bypassCheck)
				throw new ArgumentException("bypassCheck and checkFilesOnly cannot both be true.", "bypassCheck");

			if(checkFilesOnly && (noTransaction || rollback == false))
				throw new ArgumentException("When 'checkFilesOnly' is True, then 'noTran' must be False and 'rollback' must be True.", "checkFilesOnly");

			_scriptProjectFolder = scriptProjectFolder;
			_server = server;
            _connectionString = connectionString;
            _deploymentsDb = deploymentsDb;
            _noTransaction = noTransaction;
			_rollback = rollback;
			_checkFilesOnly = checkFilesOnly;
			_bypassCheck = bypassCheck;
            _forceInstall = forceInstall;

			_fileLoggerSettings = new RollingFileSettings(logFolder, "SqlScriptDeployer");
		}

		public IRollingFileSettings FileLoggerSettings { get { return _fileLoggerSettings; } }

		public string ScriptProjectFolder { get { return _scriptProjectFolder; } }

		public string Server { get { return _server; } }

        public string ConnectionString { get { return _connectionString; } }

        public string DeploymentsDb { get { return _deploymentsDb; } }

        public bool NoTransaction { get { return _noTransaction; } }

		public bool UseTransaction { get { return _noTransaction == false; } }

		public bool ShouldRollback { get { return _rollback; } }

		public bool CheckFilesOnly { get { return _checkFilesOnly; } }

		public bool BypassCheck { get { return _bypassCheck; } }

        public bool ForceInstall { get { return _forceInstall; } }

        private readonly bool _noTransaction, _checkFilesOnly, _rollback, _bypassCheck, _forceInstall;
		private readonly IRollingFileSettings _fileLoggerSettings;
        private readonly string _scriptProjectFolder, _server, _connectionString, _deploymentsDb;
    }
}