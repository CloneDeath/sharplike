using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Mono.Addins;

namespace Sharplike.Editlike.MapTools
{
	public class MapToolExtensionNode : TypeExtensionNode
	{
		[NodeAttribute]
		string icon = "";

		[NodeAttribute]
		string tooltip = "";

		[NodeAttribute]
		string tag = "";

		public String Tooltip
		{
			get { return tooltip; }
		}

		public String Tag
		{
			get { return tag; }
		}

		public Image Icon
		{
			get
			{
				if (toolIcon == null && icon != null)
				{
					using (Stream s = this.Type.Assembly.GetManifestResourceStream(icon))
					{
						toolIcon = Image.FromStream(s);
					}
				}

				return toolIcon;
			}
		}
		private Image toolIcon = null;

		public IMapTool Tool
		{
			get
			{
				if (instance == null)
					instance = (IMapTool)this.CreateInstance();
				return instance;
			}
		}
		private IMapTool instance;
	}
}
