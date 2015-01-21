// Useful C#
// Copyright (C) 2014-2015 Nicholas Randal
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
using System.Text.RegularExpressions;

namespace Randal.Sql.Scripting
{
	internal static class ScriptFormatterConstants
	{
		internal static readonly Regex
			PatternLineEndings = new Regex(@"[\t ]*\r?\n",
				RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
			UnescapedSingleQuotes = new Regex(@"'*", RegexOptions.IgnoreCase | RegexOptions.Compiled)
		;

		internal static readonly MatchEvaluator UnescapedSingleQuotesEvaluator =
			m => m.Length % 2 == 0 ? m.Value : m.Value + '\'';

		internal static readonly string
			DoubleLineBreak = Environment.NewLine + Environment.NewLine,
			LineBreakTab = Environment.NewLine + '\t'
		;

		internal const string
			Database = "Database",
			Catalog = "catalog",
			Schema = "schema",
			Version = "version",
			Body = "body",
			Header = "header",
			Needs = "needs",
			TypeTable = "Table",
			TypeView = "View",
			TypeUdf = "Udf",
			TypeSproc = "Sproc",
			Name = "name",
			NameEscaped = "nameEsc",
			Pre = "pre", Main = "main",
			Begin = "begin", End = "end",
			FuncType = "funcType", FuncMulti = "multi", FuncScalar = "scalar", FuncInline = "inline"
		;

		internal static class Script
		{
			public const string
				Sproc =
					@"{needs}--:: catalog {catalog}

--:: ignore
use {catalog}

--:: pre
exec coreCreateProcedure '{nameEsc}', '{schema}'
GO

--:: main
{header}
{body}

/*
	exec [{schema}].[{name}] {parameters}
*/",
				UserDefinedFunction =
					@"{needs}--:: catalog {catalog}

--:: ignore
use {catalog}

--:: pre
exec coreCreateFunction '{nameEsc}', '{schema}', '{funcType}'
GO

--:: main
{header}
{body}

/*
	select [{schema}].[{name}]()
*/",
				View =
					@"{needs}--:: catalog {catalog}

--:: ignore
use {catalog}

--:: pre
exec coreCreateView '{nameEsc}', '{schema}'
GO

--:: main
{header}
{body}

/*
	select top 100 * from [{schema}].[{name}]
*/",
				Table =
					@"
{needs}--:: catalog {catalog}

--:: ignore
use {catalog}

--:: pre
if(dbo.coreTableExists('{nameEsc}', '{schema}') = 0) begin

	{pre}

end

--:: main
if(dbo.coreGetTableVersion('{nameEsc}') < '{version}') begin

	{main}

	exec coreSetTableVersion '{nameEsc}', '{version}'
end

/*
	select top 100 * from [{schema}].[{name}]
*/"
				;
		}
	}
}