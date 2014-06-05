using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Sharplike.Core.Scheduling
{
	public class SimpleThreadPoolScheduler : AbstractSimpleScheduler
	{
		protected ManualResetEvent[] doneEvents;
		public override void Process()
		{
			doneEvents = new ManualResetEvent[tasks.Count];
			Int32 i = 0;
			foreach (IScheduledTask task in tasks)
			{
				doneEvents[i] = new ManualResetEvent(false);
				ThreadPool.QueueUserWorkItem(RunTask, new ThreadTask(task, doneEvents[i]));
				++i;
			}

			WaitHandle.WaitAll(doneEvents); // will wait for all events to complete
		}

		static void RunTask(Object data)
		{
			((ThreadTask)data).Task.ScheduledAction();
			((ThreadTask)data).DoneEvent.Set();
		}
	}
}
