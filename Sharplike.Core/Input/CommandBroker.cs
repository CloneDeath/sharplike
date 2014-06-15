using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace Sharplike.Core.Input
{
	public class CommandBroker : IDisposable
	{
		internal CommandControls commands = new CommandControls();

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

		public void Dispose()
		{

		}

		/// <summary>
		/// Gets or sets the current command set key. Command sets are hierarchical
		/// input systems (so that any active commands in Foo are active in Foo.Bar
		/// unless explicitly denied).
		/// 
		/// A null value or an empty string indicates to InputSystem to look only
		/// at the root command set.
		/// </summary>
		public String CommandSet
		{
			get;
			set;
		}

		private void AddHooks(AbstractInputProvider _provider)
		{
			_provider.OnKeyPressed += _provider_OnKeyPressed;
			_provider.OnKeyReleased += _provider_OnKeyReleased;
			_provider.OnMousePressed += _provider_OnMousePressed;
			_provider.OnMouseReleased += _provider_OnMouseReleased;
		}

		private void RemoveHooks(AbstractInputProvider _provider)
		{
			_provider.OnKeyPressed -= _provider_OnKeyPressed;
			_provider.OnKeyReleased -= _provider_OnKeyReleased;
			_provider.OnMousePressed -= _provider_OnMousePressed;
			_provider.OnMouseReleased -= _provider_OnMouseReleased;
		}

		void _provider_OnKeyPressed(object sender, KeyEventArgs e)
		{
			CommandData cmd = this.commands.GetCommand(e.KeyCode, this.CommandSet);
			this.StartCommand(cmd);
		}

		void _provider_OnKeyReleased(object sender, KeyEventArgs e)
		{
			CommandData cmd = this.commands.GetCommand(e.KeyCode, this.CommandSet);
			this.EndCommand(cmd);
		}

		void _provider_OnMousePressed(object sender, MouseEventArgs e)
		{
			Keys key = Keys.None;
			switch (e.Button) {
				case MouseButtons.Left:
					key = Keys.LButton;
					break;
				case MouseButtons.Middle:
					key = Keys.MButton;
					break;
				case MouseButtons.Right:
					key = Keys.RButton;
					break;
				case MouseButtons.XButton1:
					key = Keys.XButton1;
					break;
				case MouseButtons.XButton2:
					key = Keys.XButton2;
					break;
			}

			CommandData cmd = this.commands.GetCommand(key, this.CommandSet);
			this.StartCommand(cmd);
		}

		void _provider_OnMouseReleased(object sender, MouseEventArgs e)
		{
			Keys key = Keys.None;
			switch (e.Button) {
				case MouseButtons.Left:
					key = Keys.LButton;
					break;
				case MouseButtons.Middle:
					key = Keys.MButton;
					break;
				case MouseButtons.Right:
					key = Keys.RButton;
					break;
				case MouseButtons.XButton1:
					key = Keys.XButton1;
					break;
				case MouseButtons.XButton2:
					key = Keys.XButton2;
					break;
			}

			CommandData cmd = this.commands.GetCommand(key, this.CommandSet);
			this.EndCommand(cmd);
		}


		/// <summary>
		/// Performs a one-shot trigger of a given command.
		/// </summary>
		/// <param name="command">The command to trigger.</param>
		public void TriggerCommand(CommandData command)
		{
			if (command == null)
				return;
			if (this.CommandTriggered != null)
				CommandTriggered(this, new CommandEventArgs(command));
		}

		/// <summary>
		/// Starts a game command. This command will be duplicated as a trigger, and will be
		/// re-triggered as a key-repeat until it is ended by a call to EndCommand().
		/// </summary>
		/// <param name="command">The command to start.</param>
		public void StartCommand(CommandData command)
		{
			if (command == null)
				return;

			if (this.CommandStarted != null)
				CommandStarted(this, new CommandEventArgs(command));

			TriggerCommand(command);
		}

		/// <summary>
		/// Ends a command that was previously started. This will stop the command from being repeat-triggered.
		/// </summary>
		/// <param name="command">The name of the command to stop</param>
		public void EndCommand(CommandData command)
		{
			if (command == null)
				return;
			if (this.CommandEnded != null)
				CommandEnded(this, new CommandEventArgs(command));
		}

		/// <summary>
		/// Invoked when a command first reaches the InputSystem (analogous to the
		/// WinForms KeyPress event).
		/// </summary>
		public event EventHandler<CommandEventArgs> CommandTriggered;

		/// <summary>
		/// Invoked when a command begins (analogous to the WinForms KeyDown event).
		/// </summary>
		public event EventHandler<CommandEventArgs> CommandStarted;

		/// <summary>
		/// Invoked when a command begins (analogous to the WinForms KeyDown event).
		/// </summary>
		public event EventHandler<CommandEventArgs> CommandEnded;
	}
}
