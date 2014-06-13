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

		public event EventHandler<KeyEventArgs> OnKeyPressed;
		public event EventHandler<KeyEventArgs> OnKeyReleased;
		public event EventHandler<MouseEventArgs> OnMouseMove;
		public event EventHandler<MouseEventArgs> OnMousePressed;
		public event EventHandler<MouseEventArgs> OnMouseReleased;
		public event EventHandler<MouseEventArgs> OnMouseWheel;

		protected void KeyPressed(KeyEventArgs args)
		{
			if (OnKeyPressed != null) {
				OnKeyPressed(this, args);
			}
		}

		protected void KeyReleased(KeyEventArgs args)
		{
			if (OnKeyReleased != null) {
				OnKeyReleased(this, args);
			}
		}

		protected void MouseMove(MouseEventArgs args)
		{
			if (OnMouseMove != null){
				OnMouseMove(this, args);
			}
		}

		protected void MouseWheel(MouseEventArgs args)
		{
			if (OnMouseWheel != null) {
				OnMouseWheel(this, args);
			}
		}

		protected void MouseReleased(MouseEventArgs args)
		{
			if (OnMouseReleased != null) {
				OnMouseReleased(this, args);
			}
		}

		protected void MousePressed(MouseEventArgs args)
		{
			if (OnMousePressed != null) {
				OnMousePressed(this, args);
			}
		}

		public virtual void Dispose()
		{

		}
	}
}
