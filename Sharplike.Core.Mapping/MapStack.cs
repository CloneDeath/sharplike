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
		public MapStack(Size WindowSize, Int32 RegionWidth, Int32 RegionHeight, string name, AbstractRegion parent) 
			: base(name, new Vector3(RegionWidth, RegionHeight, 1), parent)
		{
			this.Size = WindowSize;
		}	
	}
}
