using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Mapping
{
	public class PageCallbackInfo
	{
		public Int64 CallTime { get; private set; }
		public IPageCallbacker Target { get; private set; }
		public PageActionDelegate Method { get; private set; }

		public PageCallbackInfo(Int64 callTime, IPageCallbacker target, PageActionDelegate method)
		{
			this.CallTime = callTime;
			this.Target = target;
			this.Method = method;
		}
	}
}
