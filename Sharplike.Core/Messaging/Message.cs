using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Core.Messaging
{
	public sealed class Message
	{
		internal Message(String name, String channel, Object[] args)
		{
			Name = name;
			Channel = channel;
			Args = args;
		}

		public readonly String Name;
		public readonly String Channel;
		public readonly Object[] Args;
	}
}
