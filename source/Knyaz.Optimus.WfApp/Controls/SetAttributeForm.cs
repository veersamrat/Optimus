﻿using System.Windows.Forms;
using HtmlElement = Knyaz.Optimus.Dom.Elements.HtmlElement;

namespace Knyaz.Optimus.WfApp.Controls
{
	public partial class SetAttributeForm : Form
	{
		public SetAttributeForm()
		{
			InitializeComponent();
		}

		public HtmlElement Element { get; set; }
		
		private void button2_Click(object sender, System.EventArgs e)
		{
			if (Element == null) return;
			var attributeName = textBoxName.Text;
			var attributeValue = textBoxValue.Text;
			Element.SetAttribute(attributeName, attributeValue);
			Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
