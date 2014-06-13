using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Sharplike.Core.Input
{
	public class InputManager
	{
		class KeyboardState
		{
			public List<Keys> PressedKeys = new List<Keys>();
			
			public void Pressed(Keys key)
			{
				if (!PressedKeys.Contains(key)) {
					PressedKeys.Add(key);
				}
			}

			public void Released(Keys key)
			{
				if (PressedKeys.Contains(key)) {
					PressedKeys.Remove(key);
				}
			}

			public bool IsDown(Keys key)
			{
				return PressedKeys.Contains(key);
			}
		}

		class MouseState
		{
			public List<MouseButtons> PressedKeys = new List<MouseButtons>();
			public int Wheel;
			public Point Position;

			public void Pressed(MouseButtons key)
			{
				if (!PressedKeys.Contains(key)) {
					PressedKeys.Add(key);
				}
			}

			public void Released(MouseButtons key)
			{
				if (PressedKeys.Contains(key)) {
					PressedKeys.Remove(key);
				}
			}

			public bool IsDown(MouseButtons key)
			{
				return PressedKeys.Contains(key);
			}
		}

		public InputManager()
		{
			CurrentKeyboardState.PressedKeys = new List<Keys>();
			PreviousKeyboardState.PressedKeys = new List<Keys>();

			CurrentMouseState.PressedKeys = new List<MouseButtons>();
			PreviousMouseState.PressedKeys = new List<MouseButtons>();
		}

		KeyboardState PreviousKeyboardState = new KeyboardState();
		KeyboardState CurrentKeyboardState = new KeyboardState();

		MouseState PreviousMouseState = new MouseState();
		MouseState CurrentMouseState = new MouseState();

		AbstractInputProvider _provider;
		internal AbstractInputProvider InputProvider
		{
			get { return _provider; }
			set
			{
				if (_provider != null) {
					RemoveHooks(_provider);
				}

				_provider = value;
				AddHooks(_provider);
			}
		}

		internal void Update()
		{
			PreviousKeyboardState.PressedKeys = new List<Keys>(CurrentKeyboardState.PressedKeys);
			PreviousMouseState.PressedKeys = new List<MouseButtons>(CurrentMouseState.PressedKeys);
		}

		private void AddHooks(AbstractInputProvider _provider)
		{
			_provider.OnKeyPressed += _provider_OnKeyPressed;
			_provider.OnKeyReleased += _provider_OnKeyReleased;
			_provider.OnMousePressed += _provider_OnMousePressed;
			_provider.OnMouseReleased += _provider_OnMouseReleased;
			_provider.OnMouseMove += _provider_OnMouseMove;
			_provider.OnMouseWheel += _provider_OnMouseWheel;
		}

		private void RemoveHooks(AbstractInputProvider _provider)
		{
			_provider.OnKeyPressed -= _provider_OnKeyPressed;
			_provider.OnKeyReleased -= _provider_OnKeyReleased;
			_provider.OnMousePressed -= _provider_OnMousePressed;
			_provider.OnMouseReleased -= _provider_OnMouseReleased;
			_provider.OnMouseMove -= _provider_OnMouseMove;
			_provider.OnMouseWheel -= _provider_OnMouseWheel;
		}

		private void _provider_OnKeyPressed(object sender, KeyEventArgs args)
		{
			CurrentKeyboardState.Pressed(args.KeyData);
		}

		private void _provider_OnKeyReleased(object sender, KeyEventArgs args)
		{
			CurrentKeyboardState.Released(args.KeyData);
		}

		private void _provider_OnMousePressed(object sender, MouseEventArgs args)
		{
			CurrentMouseState.Pressed(args.Button);
		}

		private void _provider_OnMouseReleased(object sender, MouseEventArgs args)
		{
			CurrentMouseState.Released(args.Button);
		}

		private void _provider_OnMouseMove(object sender, MouseEventArgs args)
		{
			CurrentMouseState.Position = args.Location;
		}

		private void _provider_OnMouseWheel(object sender, MouseEventArgs args)
		{
			CurrentMouseState.Wheel += args.Delta;
		}

		/* Keyboard */
		public bool IsDown(Keys key)
		{
			return CurrentKeyboardState.IsDown(key);
		}

		public bool IsUp(Keys key)
		{
			return !CurrentKeyboardState.IsDown(key);
		}

		public bool IsPressed(Keys key)
		{
			return !PreviousKeyboardState.IsDown(key) && CurrentKeyboardState.IsDown(key);
		}

		public bool IsReleased(Keys key)
		{
			return PreviousKeyboardState.IsDown(key) && !CurrentKeyboardState.IsDown(key);
		}

		/* Mouse */
		public bool IsDown(MouseButtons key)
		{
			return CurrentMouseState.IsDown(key);
		}

		public bool IsUp(MouseButtons key)
		{
			return !CurrentMouseState.IsDown(key);
		}

		public bool IsPressed(MouseButtons key)
		{
			return !PreviousMouseState.IsDown(key) && CurrentMouseState.IsDown(key);
		}

		public bool IsReleased(MouseButtons key)
		{
			return PreviousMouseState.IsDown(key) && !CurrentMouseState.IsDown(key);
		}

		public int MouseWheel
		{
			get { return CurrentMouseState.Wheel - PreviousMouseState.Wheel; }
		}

		public Point MousePosition {
			get { return CurrentMouseState.Position; }
		}

		public Point PreviousMosePosition {
			get { return PreviousMouseState.Position; }
		}

		internal IList<Keys> GetAllPressed()
		{
			List<Keys> ret = new List<Keys>();
			foreach (Keys key in CurrentKeyboardState.PressedKeys) {
				if (this.IsPressed(key)) {
					ret.Add(key);
				}
			}
			return ret;
		}
	}
}
