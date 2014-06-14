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

		/// <summary>
		/// Triggered whenever a keyboard key is pressed.
		/// </summary>
		public event EventHandler<KeyEventArgs> OnKeyPressed;

		/// <summary>
		/// Triggered whenever a keyboard key is released.
		/// </summary>
		public event EventHandler<KeyEventArgs> OnKeyReleased;

		/// <summary>
		/// Triggered whenever the mouse moves.
		/// </summary>
		public event EventHandler<MouseEventArgs> OnMouseMove;

		/// <summary>
		/// Triggered whenever a mouse button is pressed.
		/// </summary>
		public event EventHandler<MouseEventArgs> OnMousePressed;

		/// <summary>
		/// Triggered whenever a mouse button is released.
		/// </summary>
		public event EventHandler<MouseEventArgs> OnMouseReleased;

		/// <summary>
		/// Triggered whenever the mouse wheel is scrolled.
		/// </summary>
		public event EventHandler<MouseEventArgs> OnMouseWheel;

		/// <summary>
		/// Triggers the OnKeyPressed event.
		/// </summary>
		/// <param name="args">Event data to use</param>
		protected void KeyPressed(KeyEventArgs args)
		{
			if (OnKeyPressed != null) {
				OnKeyPressed(this, args);
			}
		}

		/// <summary>
		/// Triggers the OnKeyReleased event.
		/// </summary>
		/// <param name="args">Event data to use</param>
		protected void KeyReleased(KeyEventArgs args)
		{
			if (OnKeyReleased != null) {
				OnKeyReleased(this, args);
			}
		}

		/// <summary>
		/// Triggers the OnMouseMove event.
		/// </summary>
		/// <param name="args">Event data to use</param>
		protected void MouseMove(MouseEventArgs args)
		{
			if (OnMouseMove != null){
				OnMouseMove(this, args);
			}
		}

		/// <summary>
		/// Triggers the OnMouseWheel event.
		/// </summary>
		/// <param name="args">Event data to use</param>
		protected void MouseWheel(MouseEventArgs args)
		{
			if (OnMouseWheel != null) {
				OnMouseWheel(this, args);
			}
		}

		/// <summary>
		/// Triggers the OnMouseReleased event.
		/// </summary>
		/// <param name="args">Event data to use</param>
		protected void MouseReleased(MouseEventArgs args)
		{
			if (OnMouseReleased != null) {
				OnMouseReleased(this, args);
			}
		}

		/// <summary>
		/// Triggers the OnMousePressed event.
		/// </summary>
		/// <param name="args">Event data to use</param>
		protected void MousePressed(MouseEventArgs args)
		{
			if (OnMousePressed != null) {
				OnMousePressed(this, args);
			}
		}

		/// <summary>
		/// Disposes all unmanaged input resources.
		/// </summary>
		public virtual void Dispose()
		{

		}
	}
}
