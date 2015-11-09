﻿// Useful C#
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

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Randal.Core.T4;
using Randal.Core.Testing.UnitTest;

namespace Randal.Tests.Core.T4
{
	[TestClass]
	public sealed class CodeGeneratorTests : BaseUnitTest<CodeGeneratorThens>
	{
		[TestMethod, PositiveTest]
		public void ShouldHaveValidInstance_WhenCreating()
		{
			When(Creating);

			Then.Target.Should().NotBeNull().And.BeAssignableTo<ICodeGenerator>();
		}

		[TestMethod, PositiveTest]
		public void ShouldHaveListOfCodeDefintions_WhenGeneratingList()
		{
			Given.CodeDefinitions = new[]
			{
				new DbCodeDefinition("1", "Visible", "Component Visible", "Now you see it."),
				new DbCodeDefinition("0", "Hidden", "Component Hidden", "Now you don't.")
			};

			When(GeneratingList);

			Then.Lines.Should().HaveCount(5);
			Then.Lines[0].Should().Be("[Display(Name = \"Component Visible\", Description = \"Now you see it.\")]");
			Then.Lines[1].Should().Be("Visible = 1,");
			Then.Lines.Last().Should().NotBeEmpty();
			Then.Lines.Last().Should().NotEndWith(",");
		}

		private void GeneratingList()
		{
			IReadOnlyList<DbCodeDefinition> codes = Given.CodeDefinitions;
			Then.Lines = codes.ToCodeLines();
		}

		protected override void Creating()
		{
			Then.Target = new CodeGenerator("Data Source=.;Integrated Security=true;Initial Catalog=master;");
		}
	}

	public sealed class CodeGeneratorThens
	{
		public CodeGenerator Target;
		public IReadOnlyList<string> Lines;
	}
}
