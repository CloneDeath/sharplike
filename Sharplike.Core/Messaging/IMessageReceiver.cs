using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Core.Messaging
{
    public interface IMessageReceiver
    {
        void OnMessage(Message msg);
		void AssertArgumentTypes(Message msg);
    }
}
