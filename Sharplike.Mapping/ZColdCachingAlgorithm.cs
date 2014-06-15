using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Mapping
{
	public class ZColdCachingAlgorithm : AbstractCachingAlgorithm
	{
		public Int32 ActiveLevel;

		public ZColdCachingAlgorithm(AbstractMap map)
			: base(map)
		{
			ActiveLevel = 0;
		}

		public override void AssessCache()
		{
			List<AbstractPage> pages = new List<AbstractPage>();

			foreach (AbstractPage page in m_map.Pages.Values)
			{
				pages.Add(page);
			}

			foreach (AbstractPage page in pages)
			{
				if (page.address.Z == this.ActiveLevel)
				{
					this.SetPageMode(page, CachingMode.Active);
				}
				else
				{
					this.SetPageMode(page, CachingMode.Cold);
				}
			}

		}
	}
}
