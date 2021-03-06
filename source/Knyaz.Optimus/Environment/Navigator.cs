﻿using System.Globalization;
using Knyaz.Optimus.Dom.Interfaces;

namespace Knyaz.Optimus.Environment
{
	/// <summary>
	/// http://www.w3schools.com/jsref/obj_navigator.asp
	/// </summary>
	public class Navigator : INavigator, INavigatorPlugins
	{
		private readonly INavigatorPlugins _plugins;

		public Navigator(INavigatorPlugins plugins)
		{
			_plugins = plugins;
		}
		
		public string AppCodeName { get { return "Optimus Browser"; } }
		public string AppName { get { return "Optimus"; } }

		public string AppVersion
		{
			get
			{
				var ver = GetType().Assembly.GetName().Version;
				return ver.Major+"."+ver.MajorRevision;
			}
		}

		public bool CookieEnabled{get{ return true; }}
		public string Geolocation{get { return null; /*todo*/ }}
		public bool OnLine{get { return true; }}
		public string Platform{get { return ".NET"; /*todo*/ }}
		public string Product { get { return "Optimus"; } }
		public string UserAgent { get; internal set; }

		public string Language => CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

		public bool JavaEnabled() => true;
		
		public MimeTypesArray MimeTypes => _plugins.MimeTypes;

		public PluginsArray Plugins => _plugins.Plugins;
	}
}
