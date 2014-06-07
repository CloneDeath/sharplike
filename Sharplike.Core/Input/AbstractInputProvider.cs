using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sharplike.Core.Input
{
	/// <summary>
	/// The base class for an input provider. The provider should trigger each event as soon as they
	/// can. If the implementation is not event based, then the Poll method is where state changes
	/// should be monitored.
	/// </summary>
	public abstract class AbstractInputProvider : IDisposable
	{
		/// <summary>
		/// An optional chance to poll the current state of input devices, and trigger the appropriate events.
		/// If your implementation is event based, then this option can be ignored.
		/// </summary>
		public virtual void Poll() { }

		public KeyEventHandler OnKeyPressed;
		public KeyEventHandler OnKeyReleased;
		public MouseEventHandler OnMouseMove;
		public MouseEventHandler OnMousePressed;
		public MouseEventHandler OnMouseReleased;
		public MouseEventHandler OnMouseWheel;

		public abstract void Dispose();
	}
}
