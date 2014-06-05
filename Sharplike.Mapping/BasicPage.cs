using System;

namespace Sharplike.Mapping
{
	[Serializable]
	public class BasicPage : AbstractPage
	{
		public BasicPage (Vector3 pageSize) 
			: base(pageSize)
		{
		}
		public BasicPage(Int32 x, Int32 y, Int32 z)
			: base(x, y, z)
		{
		}
		public override void Build ()
		{
		}
	}
}
