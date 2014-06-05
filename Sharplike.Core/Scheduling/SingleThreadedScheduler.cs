using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Sharplike.Core.Scheduling
{
	public class SingleThreadedScheduler : AbstractSimpleScheduler
	{
		public override void Process()
		{
			foreach (IScheduledTask t in tasks)
			{
				t.ScheduledAction();
			}
		}
	}
}
