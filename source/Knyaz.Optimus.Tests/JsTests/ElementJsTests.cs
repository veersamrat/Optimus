﻿using NUnit.Framework;

namespace Knyaz.Optimus.Tests.JsTests
{
	/// <summary>
	/// This stuff runs JS tests from Resources/JsTests/ElementTetst.js file.
	/// </summary>
	[TestFixture]
	public class ElementJsTests
    {
		[TestCase("SetParent")]
		[TestCase("SetOwnerDocument")]
		[TestCase("Remove")]
		[TestCase("AttributesLength")]
		[TestCase("AttributesGetByName")]
		[TestCase("SetOnClickAttribute")]
		[TestCase("SetOnClickAttributePropogation")]
		[TestCase("EventHandlingOrder")]
		[TestCase("EventListenerParams")]
		[TestCase("EventHandlerParams")]
		[TestCase("AttrEventHandlerParams")]
		public void ElementTests(string testName) => JsTestsRunner.Run("ElementTests", testName);
	}
}
