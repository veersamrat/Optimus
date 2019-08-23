﻿using Knyaz.Optimus.Dom.Css;
using System;
using Knyaz.Optimus.Environment;

namespace Knyaz.Optimus.Dom.Interfaces
{
	public interface IWindow : IEventTarget
	{
		int InnerWidth { get; set; }
		int InnerHeight { get; set; }

		IScreen Screen { get; }
		Location Location { get; }
		INavigator Navigator { get; }
		IHistory History { get; }

		int SetTimeout(Action<object[]> handler, double? delay, params object[] data);
		void ClearTimeout(int handle);
		int SetInterval(Action<object[]> handler, double? delay, params object[] data);
		void ClearInterval(int handle);

		ICssStyleDeclaration GetComputedStyle(IElement element);
		ICssStyleDeclaration GetComputedStyle(IElement element, string pseudoElt);
		MediaQueryList MatchMedia(string query);
	}
}
