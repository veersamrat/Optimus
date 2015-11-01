﻿using System;
using System.Linq;
using WebBrowser.ScriptExecuting;

namespace WebBrowser.Dom.Elements
{
	/// <summary>
	/// http://www.w3.org/TR/2012/WD-html5-20121025/elements.html#htmlelement
	/// </summary>
	public class HtmlElement : Element, IHtmlElement
	{
		static class Defaults
		{
			public static bool Hidden = false;
		}

		public HtmlElement(Document ownerDocument, string tagName)
			: base(ownerDocument, tagName)
		{
			Style = new CssStyleDeclaration();
		}

		public bool Hidden
		{
			get { return GetAttribute("hidden", Defaults.Hidden); }
			set { SetAttribute("hidden", value.ToString());}
		}

		public void Click()
		{
			var evt = OwnerDocument.CreateEvent("Event");
			evt.InitEvent("click", true, true);
			DispatchEvent(evt);
		}

		public event Action OnClick;

		public override bool DispatchEvent(Event evt)
		{
			if (evt.Type == "click" && OnClick != null)
				OnClick();

			
			return base.DispatchEvent(evt);
		}

		public CssStyleDeclaration Style { get; private set; }

		protected override void UpdatePropertyFromAttribute(string value, string invariantName)
		{
			base.UpdatePropertyFromAttribute(value, invariantName);

			//todo: remove it to property
			if (invariantName == "style")
			{
				if (!string.IsNullOrEmpty(value))
				{
					var styleParts = value.Split(';');
					foreach (var stylePart in styleParts.Where(s => !string.IsNullOrEmpty(s)))
					{
						var keyValue = stylePart.Split(':');
						if (keyValue.Length == 2)
						{
							//todo: handle duplicates
							Style.Properties.Add(keyValue[0], keyValue[1]);
						}
					}
				}
			} 
		}
	}

	[DomItem]
	public interface IHtmlElement
	{
		bool Hidden { get; set; }
		void Click();
		event Action OnClick;
	}
}
