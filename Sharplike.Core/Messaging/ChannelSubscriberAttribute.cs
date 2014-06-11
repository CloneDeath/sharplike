using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Core.Messaging
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public class ChannelSubscriberAttribute : Attribute
	{
		public ChannelSubscriberAttribute(String channelname)
		{
			Channel = channelname;
		}

		public String Channel
		{
			get;
			private set;
		}
	}
}
