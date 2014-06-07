using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharplike.Core.Input
{
	public sealed class CommandEventArgs : EventArgs
	{
		public CommandEventArgs(CommandData cmdData)
		{
			Handled = false;
			CommandData = cmdData;
		}

		public Boolean Handled
		{
			get;
			set;
		}

		public CommandData CommandData
		{
			get;
			private set;
		}
	}
}
