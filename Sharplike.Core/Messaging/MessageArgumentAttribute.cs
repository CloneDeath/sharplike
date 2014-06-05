using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Core.Messaging
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
	public class MessageArgumentAttribute : Attribute
	{
		public MessageArgumentAttribute(int argindex, Type expectedtype)
		{
			ArgumentIndex = argindex;
			ArgumentType = expectedtype;
		}

		public int ArgumentIndex
		{
			get;
			private set;
		}

		public Type ArgumentType
		{
			get;
			private set;
		}
	}
}
