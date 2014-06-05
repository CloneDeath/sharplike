using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace Sharplike.Core.Input
{
    public abstract class AbstractInputProvider : IDisposable
    {
        public abstract void Poll();
        public abstract Point GetMousePosition();

        public InputSystem System
        {
            get;
            internal set;
        }

		protected void KeyDown(Keys keycode)
        {
			CommandData cmd = System.rootcstate.GetCommand(keycode, System.CommandSetKey);
            System.StartCommand(cmd);
        }

        protected void KeyUp(Keys keycode)
        {
            CommandData cmd = System.rootcstate.GetCommand(keycode, System.CommandSetKey);
            System.EndCommand(cmd);
        }

        protected void KeyPress(Keys keycode)
        {
            CommandData cmd = System.rootcstate.GetCommand(keycode, System.CommandSetKey);
            System.TriggerCommand(cmd);
        }

        protected void MouseDown(Keys k, Point screenCoords, Point tileCoords,
			Boolean shift, Boolean control, Boolean alt)
        {
			if (shift)
				k = k | Keys.Shift;
			if (control)
				k = k | Keys.Control;
			if (alt)
				k = k | Keys.Alt;

			CommandData cmd = System.rootcstate.GetCommand(k, System.CommandSetKey, true);
			System.StartCommand(cmd);
        }

		protected void MouseUp(Keys key, Point screenCoords, Point tileCoords,
			Boolean shift, Boolean control, Boolean alt)
        {
			if (shift)
				key = key | Keys.Shift;
			if (control)
				key = key | Keys.Control;
			if (alt)
				key = key | Keys.Alt;

			CommandData cmd = System.rootcstate.GetCommand(key, System.CommandSetKey, true);
			System.EndCommand(cmd);
        }

		protected void MouseWheel(Keys key, Point screenCoords, Point tileCoords,
			Boolean shift, Boolean control, Boolean alt)
		{
			if (shift)
				key = key | Keys.Shift;
			if (control)
				key = key | Keys.Control;
			if (alt)
				key = key | Keys.Alt;

			CommandData cmd = System.rootcstate.GetCommand(key, System.CommandSetKey, true);
			System.StartCommand(cmd);
			System.EndCommand(cmd);
		}

        public abstract void Dispose();
    }
}
