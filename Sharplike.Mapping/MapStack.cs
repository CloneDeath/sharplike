using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sharplike.Core.Rendering;

namespace Sharplike.Mapping
{
	[Serializable]
	public class MapStack : AbstractMap
	{
		public MapStack(Size regionsize, Int32 width, Int32 height, string name, AbstractRegion parent) 
			: base(new Vector3(width, height, 1), name, parent)
		{
			this.Size = regionsize;
		}	
	}
}
