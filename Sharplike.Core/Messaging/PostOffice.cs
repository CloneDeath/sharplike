using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Sharplike.Core.Messaging
{
	internal class PostOffice
	{
		internal void EnqueueMessage(IMessageReceiver target, Message msg)
		{
			List<Message> mailbox;
			lock (inbox)
			{
				if (!inbox.TryGetValue(target, out mailbox))
				{
					mailbox = new List<Message>();
					inbox[target] = mailbox;
				}
			}
			lock (mailbox)
			{
				mailbox.Add(msg);
			}
		}

		internal void RemoveReceiver(IMessageReceiver r)
		{
			lock (inbox)
			{
				inbox.Remove(r);
			}
		}

		internal void PumpMessages()
		{
			Dictionary<IMessageReceiver, List<Message>> box;
			lock (inbox)
			{
				 box = new Dictionary<IMessageReceiver, List<Message>>(inbox);
			}

			foreach (KeyValuePair<IMessageReceiver, List<Message>> kvp in box)
			{
				lock (kvp.Value)
				{
					foreach (Message m in kvp.Value)
					{
						kvp.Key.OnMessage(m);
					}
					kvp.Value.Clear();
				}
			}
		}

		private Dictionary<IMessageReceiver, List<Message>> inbox = new Dictionary<IMessageReceiver, List<Message>>();
	}
}
