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

using System;
using System.Fakes;
using FluentAssertions;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Randal.Core.IO.Logging;
using Randal.Core.Testing.UnitTest;

namespace Randal.Tests.Core.IO.Logging
{
	[TestClass]
	public sealed class LogExceptionEntryTests : BaseUnitTest<UnitTest2Thens>
	{
		[TestInitialize]
		public override void Setup()
		{
			base.Setup();

			Given.SystemDateTime = new DateTime();
		}

		[TestMethod]
		public void ShouldHaveValidEntryWhenCreating()
		{
			Given.Exception = new InvalidTimeZoneException();
			Given.SystemDateTime = new DateTime(2014, 6, 13, 1, 2, 3);

			WhenCreating();

			Then.Entry.Should().NotBeNull().And.BeAssignableTo<ILogEntry>();
			Then.Entry.VerbosityLevel.Should().Be(Verbosity.Vital);
			Then.Entry.ShowTimestamp.Should().BeTrue();
			Then.Entry.Timestamp.Should().Be(new DateTime(2014, 6, 13, 1, 2, 3));
			Then.Entry.Message.Should().BeEmpty();
		}

		private void WhenCreating()
		{
			using (ShimsContext.Create())
			{
				ShimDateTime.NowGet = () => Given.SystemDateTime;

				if (Given.TestForMember("Message"))
					Then.Entry = new ExceptionEntry(Given.Exception, Given.Message);
				else
					Then.Entry = new ExceptionEntry(Given.Exception);
			}
		}
	}

	public sealed class UnitTest2Thens
	{
		public ExceptionEntry Entry;
	}
}