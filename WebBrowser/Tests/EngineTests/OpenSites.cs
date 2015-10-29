﻿using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace WebBrowser.Tests.EngineTests
{
	[TestFixture]
	public class OpenSites
	{
		[TestCase("http://okkamtech.com")]
		[TestCase("http://ya.ru")]
		[TestCase("http://redmine.todosoft.org")]
		[TestCase("http://google.com")]
		[TestCase("https://html5test.com")]
		public void OpenUrl(string url)
		{
			var engine = new Engine();
			engine.OpenUrl(url);
		}

		[Test]
		public void Html5Score()
		{
			var engine = new Engine();
			engine.OpenUrl("https://html5test.com");
			Thread.Sleep(10000);//wait calculation

			var score = engine.Document.GetElementById("score");
			Assert.IsNotNull(score, "score");
			var tagWithValue = score.GetElementsByTagName("strong").FirstOrDefault();
			Assert.IsNotNull(tagWithValue, "strong");
			System.Console.WriteLine("Score: " + tagWithValue.InnerHTML);

			foreach (var category in ("parsing elements form location output input communication webrtc interaction " +
			                         "performance security history offline storage files streams video audio responsive " +
			                         "canvas webgl animation").Split(' '))
			{
				try
				{
					System.Console.WriteLine(category + ": " +
					                         engine.Document.GetElementById("head-" + category).GetElementsByTagName("span")[0]
						                         .InnerHTML);
				}
				catch (Exception)
				{
					System.Console.WriteLine(category + " not found");
				}
			}
		}

		[Test]
		public void BrowseOkkam()
		{
			var engine = new Engine();
			engine.ScriptExecutor.OnException += exception => System.Console.WriteLine(exception.ToString());
			engine.OpenUrl("http://okkamtech.com");
			Thread.Sleep(10000);
			var userName = engine.Document.GetElementById("UserName");
			Assert.IsNotNull(userName);
		}

		[Test]
		public void BrowseKwinto()
		{
			var engine = new Engine();
			engine.ScriptExecutor.OnException += exception => System.Console.WriteLine(exception.ToString());
			engine.OpenUrl("http://192.168.1.36:8891");
			Thread.Sleep(10000);
			var userName = engine.Document.GetElementById("UserName");

			Assert.IsNotNull(userName);
		}
	}
}
