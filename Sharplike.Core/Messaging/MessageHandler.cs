using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Sharplike.Core.Messaging
{
	[Serializable]
	public class MessageHandler
	{
		public delegate void HandlerFunction(Message msg);
		private Dictionary<String, HandlerFunction> handlers = new Dictionary<string, HandlerFunction>();

		public bool HandleMessage(Message msg)
		{
			HandlerFunction func;
			if (handlers.TryGetValue(msg.Name, out func))
			{
				func(msg);
				return true;
			}
			return false;
		}

		public void AssertArgumentTypes(Message msg)
		{
			HandlerFunction func;
			if (handlers.TryGetValue(msg.Name, out func))
			{
				foreach (MessageArgumentAttribute attr in Attribute.GetCustomAttributes(func.GetType()))
				{
					if (msg.Args.Length <= attr.ArgumentIndex)
					{
						throw new ArgumentException(
							String.Format("Argument {0}: Expected argument of type {1}.",
							attr.ArgumentIndex, attr.ArgumentType.FullName));
					}

					if (attr.ArgumentType.IsAssignableFrom(msg.Args[attr.ArgumentIndex].GetType()))
					{
						throw new ArgumentException(
							String.Format("Argument {0}: Expected argument of type {1}.",
							attr.ArgumentIndex, attr.ArgumentType.FullName));
					}
				}
			}
		}

		public void SetHandler(String message, HandlerFunction del)
		{
			handlers[message] = del;
		}

		public void RemoveHandler(String message)
		{
			handlers.Remove(message);
		}
	}
}
