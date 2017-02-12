﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Knyaz.Optimus.Dom.Elements;
using Knyaz.Optimus.Html;
using HtmlElement = Knyaz.Optimus.Html.HtmlElement;
using IHtmlElement = Knyaz.Optimus.Html.IHtmlElement;

namespace Knyaz.Optimus.Dom
{
	//Build document using own parser
	internal class DocumentBuilder
	{
		public static void Build(Node parentNode, string htmlString, NodeSources source = NodeSources.DocumentBuilder)
		{
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(htmlString)))
			{
				var html = HtmlParser.Parse(stream);
				BuildInternal(parentNode, html, source);
			}
		}

		private static IEnumerable<IHtmlNode> ExpandHtmlTag(IEnumerable<IHtmlNode> parse)
		{
			foreach (var htmlNode in parse)
			{
				var tag = htmlNode as HtmlElement;
				if (tag != null && tag.Name.ToLowerInvariant() == "html")
				{
					foreach (var child in tag.Children)
					{
						yield return child;
					}
				}
				else
				{
					yield return htmlNode;
				}
			}
		}

		public static void Build(Document document, Stream stream)
		{
			var html = HtmlParser.Parse(stream);
			Build(document, html);
		}

		public static void Build(Document document, IEnumerable<IHtmlNode> htmlElements)
		{
			BuildInternal(document.DocumentElement, ExpandHtmlTag(htmlElements));
		}

		private static void BuildInternal(Node parentNode, IEnumerable<IHtmlNode> htmlElements, NodeSources source = NodeSources.DocumentBuilder)
		{
			foreach (var htmlElement in htmlElements)
			{
				BuildElem(parentNode, htmlElement, source);
			}
		}

		private static void BuildElem(Node node, IHtmlNode htmlNode, NodeSources source, Node insertBefore = null)
		{
			var docType = htmlNode as HtmlDocType;
			if (docType != null)
			{
				//todo: may be it's wrong to assume the doctype element placed before html in source document
				node.OwnerDocument.InsertBefore(new DocType(), node.OwnerDocument.DocumentElement);
			}
			
			var comment = htmlNode as HtmlComment;
			if (comment != null)
			{
				node.AppendChild(node.OwnerDocument.CreateComment(comment.Text));
				return;
			}

			var htmlElement = htmlNode as IHtmlElement;

			var htmlHtmlElt = node as HtmlHtmlElement;
			if (htmlHtmlElt != null && (htmlElement == null || (
				!htmlElement.Name.Equals(TagsNames.Body, StringComparison.InvariantCultureIgnoreCase) && 
				!htmlElement.Name.Equals(TagsNames.Head, StringComparison.InvariantCultureIgnoreCase))))
			{
				node = node.OwnerDocument.Body;
			}

			var txt = htmlNode as IHtmlText;
			if (txt != null)
			{
				var c = node.OwnerDocument.CreateTextNode(txt.Value);
				c.Source = source;

				node.AppendChild(c);
				return;
			}

			
			if (htmlElement == null)
				return;

			Element elem = null;
			var append = false;

			if (htmlHtmlElt != null)
			{
				var invariantName = htmlElement.Name.ToUpperInvariant();
				if (invariantName == "HEAD" || invariantName == "BODY")
					elem = htmlHtmlElt.GetElementsByTagName(invariantName).FirstOrDefault();
			}

			//if parent is table
			var table = node as HtmlTableElement;
			var elementInvariantName = htmlElement.Name.ToUpperInvariant();
			if (table != null)
			{
				if(elementInvariantName == TagsNames.Tr)
					elem = table.InsertRow();
				else if (elementInvariantName == TagsNames.Col)
				{
					var colgroup = node.OwnerDocument.CreateElement(TagsNames.Colgroup);
					table.AppendChild(colgroup);
					node = colgroup;
				}
				else if (node.ParentNode != null && elementInvariantName != TagsNames.TBody &&
				         elementInvariantName != TagsNames.Tr &&
				         elementInvariantName != TagsNames.Caption &&
				         elementInvariantName != TagsNames.THead &&
				         elementInvariantName != TagsNames.TFoot &&
				         elementInvariantName != TagsNames.Colgroup &&
						 elementInvariantName != TagsNames.Col)
				{
					BuildElem(node.ParentNode, htmlNode, source, node);
					return;
				}
			}

			if (elem == null)
			{
				append = true;
				elem = node.OwnerDocument.CreateElement(htmlElement.Name);	
			}

			elem.Source = source;
			
			if (elem is Script)
			{
				var htmlText = htmlElement.Children.FirstOrDefault() as IHtmlText;
				elem.InnerHTML = htmlText != null ? htmlText.Value : string.Empty;
			}
			
			foreach (var attribute in htmlElement.Attributes)
			{
				elem.SetAttribute(attribute.Key, attribute.Value);
			}

			if (append)
			{
				if (insertBefore != null)
					node.InsertBefore(elem, insertBefore);
				else
					node.AppendChild(elem);
			}

			BuildInternal(elem, htmlElement.Children, source);
		}
	}
}
