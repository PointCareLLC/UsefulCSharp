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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Randal.Core.Dynamic;

namespace Randal.Core.Testing.UnitTest
{
	[TestClass]
	public abstract class BaseUnitTest<TThens> where TThens : class, new()
	{
		[TestInitialize]
		public void Setup()
		{
			Given = Given ?? new DynamicEntity(MissingMemberBehavior.ReturnsNull);
			Then = new TThens();

			OnSetup();
		}

		[TestCleanup]
		public void Teardown()
		{
			OnTeardown();

			ThenLastAction = null;

			var disposeMe = Then as IDisposable;
			if (disposeMe != null)
				disposeMe.Dispose();

			Given.Clear();
			Then = null;
		}

		protected virtual void OnSetup() { }

		protected virtual void OnTeardown() { }

		/// <summary>
		/// Dyanmic object to receive any necessary Given data for the current test.
		/// </summary>
		protected dynamic Given;

		/// <summary>
		/// Determine if all provided members have been defined as Given values.
		/// </summary>
		/// <param name="members">A list of property names</param>
		/// <returns>True if all properties specified are defined, otherwise False.</returns>
		protected bool GivensDefined(params string[] members)
		{
			if (members.Length == 0)
				return true;

			return members.All(member => Given.TestForMember(member));
		}

		/// <summary>
		/// Will execute each action provided, in order.  If Creating was not provided as an action, Creating will be called automatically as the first action.
		/// However, if a Creating is provided then it will not be called automatically and it is assumed that the caller wants full control of actions and the order.
		/// </summary>
		/// <param name="actions">A list of actions to be performed for the current test</param>
		protected void When(params Action[] actions)
		{
			if (actions.Any(a => a == Creating) == false)
				Creating();

			foreach (var action in actions)
				action();
		}

		/// <summary>
		/// Will execute each action provided, in order, except for the last one.  That action will be set as ThenLastAction and will not be executed.
		/// This is done so that the action can be executed in conjunction with an assertion mechanism other than MSTest's ExpectedException attribute.
		/// </summary>
		/// <param name="actions"></param>
		protected void ThrowsExceptionWhen(params Action[] actions)
		{
			var listOfActions = actions.ToList();

			if (actions.Any(a => a == Creating) == false)
				listOfActions.Insert(0, Creating);

			for (var n = 0; n < listOfActions.Count - 1; n++)
				listOfActions[n]();

			ThenLastAction = listOfActions.Last();
		}

		protected abstract void Creating();

		protected TThens Then;
		protected Action ThenLastAction { get; private set; }
	}
}