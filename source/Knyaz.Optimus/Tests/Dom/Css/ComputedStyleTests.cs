﻿#if NUNIT
using System.IO;
using System.Linq;
using System.Text;
using Knyaz.Optimus.TestingTools;
using NUnit.Framework;

namespace Knyaz.Optimus.Tests.Dom.Css
{
	[TestFixture]
	public class ComputedStyleTests
	{
		private Engine Load(string html)
		{
			var engine = new Engine() { ComputedStylesEnabled = true };
			engine.Load(new MemoryStream(Encoding.UTF8.GetBytes(html)));
			return engine;
		}

		[Test]
		public void ComputedStyleTest()
		{
			var engine =
				Load(
					"<head><style>div{display:inline-block}.a{width:100px;height:100px}</style></head><body><div class=a id=d></div></body>");
			var lastStyleSheet = engine.Document.StyleSheets.Last();
			Assert.AreEqual(2, lastStyleSheet.CssRules.Count);
			var div = engine.Document.GetElementById("d");
			engine.Window.GetComputedStyle(div).Assert(style => style.GetPropertyValue("width") == "100px");
		}

		[Test]
		public void ComputedStyleIsAlwaysActual()
		{
			var engine = Load("<head><style>div{color:red}</style><body><div id=d></div>");
			var doc = engine.Document;
			var div = doc.GetElementById("d");
			var divStyle = div.GetComputedStyle();
			Assert.AreEqual("red", divStyle.GetPropertyValue("color"));
			var newStyle = doc.CreateElement("style");
			newStyle.InnerHTML = "div{color:green}";
			doc.Head.AppendChild(newStyle);
			Assert.AreEqual("green", divStyle.GetPropertyValue("color"));
		}

		[Test]
		public void SpecificHtml5TestCom()
		{
			var engine = Load(@"<head><style>.pointsPanel h2 > strong {
			font - size: 3.8em;
		}</style></head><body><div class=""pointsPanel""><h2><strong id=test></strong></h2></div></body>");
			var doc = engine.Document;
			var elt = doc.GetElementById("test");
			elt.GetComputedStyle().Assert(style => style.GetPropertyValue("font-size") == "3.8em");
		}

		[Test]
		public void GetDisplayDefaultStyle()
		{
			var engine = Load("<body><div id=d><span id=s>ABC</span><span>123</span></div></body>");
			var spanDisplay = engine.Document.GetElementById("s").GetComputedStyle().GetPropertyValue("display");
			Assert.AreEqual(spanDisplay, "inline");
			var divDislpay = engine.Document.GetElementById("d").GetComputedStyle().GetPropertyValue("display");
			Assert.AreEqual(divDislpay, "block");
		}

		[Test]
		public void InheritTest()
		{
			var engine = Load("<body style='color:red'><div id=d style='color:inherit'></div></body>");
			var div = engine.Document.GetElementById("d");
			var color = div.GetComputedStyle().GetPropertyValue("color");
			Assert.AreEqual("red", color);
		}
	}
}
#endif