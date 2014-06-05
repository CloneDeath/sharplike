using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Core.Scheduling
{
	public interface IScheduler
	{
		void AddTask(IScheduledTask task);
		Boolean RemoveTask(IScheduledTask task);
		void ClearTasks();

		void Process();
	}
}
