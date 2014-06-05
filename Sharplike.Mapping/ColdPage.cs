using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Mapping
{
	public sealed class ColdPage : AbstractPage
	{
		public ColdPage(Vector3 size)
		{
			cacheMode = CachingMode.Cold;
			this.size = size;
		}

		public override void Build()
		{
		}

	}
}
