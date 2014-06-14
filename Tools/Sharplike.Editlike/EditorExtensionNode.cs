using System;
using System.Collections.Generic;
using System.Text;
using Mono.Addins;

namespace Sharplike.Editlike
{
	/// <summary>
	/// A factory class for squares and entities. New factories are defined in the .addin file
	/// for your project.
	/// </summary>
	public class EditorExtensionNode : TypeExtensionNode
	{
		//[NodeAttribute]
		//string editor = "";

		[NodeAttribute]
		int gid = -1;

		[NodeAttribute]
		string tooltip = "";

		/// <summary>
		/// The tooltip for this particular editor extension.
		/// </summary>
		public String TooltipText
		{
			get { return tooltip; }
		}

		/// <summary>
		/// The index of the glyph that will represent this factory in the editor.
		/// </summary>
		public int GlyphID
		{
			get
			{
				return gid;
			}
		}
	}
}
