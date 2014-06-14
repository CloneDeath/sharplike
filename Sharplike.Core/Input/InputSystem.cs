using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.IO;
using Nini.Ini;

namespace Sharplike.Core.Input
{
	/// <summary>
	/// Responsible for handling and organizing the data associated with user input.
	/// Keeps track of input state and triggered commands.
	/// </summary>
	public sealed class InputSystem
	{
		internal Dictionary<String, String> winEvents = new Dictionary<string, string>();
		public CommandBroker Command { get; private set; }
		public InputManager Input { get; private set; }

		private AbstractInputProvider _provider;
		public AbstractInputProvider InputProvider
		{
			get
			{
				return _provider;
			}
			internal set
			{
				_provider = value;
				Command.InputProvider = value;
				Input.InputProvider = value;
			}
		}

		
		internal InputSystem() {
			Command = new CommandBroker();
			Input = new InputManager();
		}

		/// <summary>
		/// Loads control bindings from a specified path.
		/// </summary>
		/// <param name="filename">The path to load from. Note: This does NOT automatically use Game.PathTo()</param>
		public void LoadConfiguration(String filename)
		{
			using (FileStream fs = new FileStream(filename, FileMode.Open)) {
				using (IniReader r = new IniReader(fs)) {
					while (true) {
						if (r.Type != IniType.Section) {
							if (!r.MoveToNextSection())
								break;
						}
						String[] parts = r.Name.Split(' ');
						if (parts[0] == "KeyBindings") {
							if (parts.Length > 1) {
								CommandControls cs = Command.commands.GetChild(parts[1], true);
								cs.ReadIni(r);
							} else {
								Command.commands.ReadIni(r);
							}
						}
						/*else if (parts[0] == "MouseButtons")
						{
						}*/
						else if (parts[0] == "WindowEvents") {
							while (r.Read()) {
								if (r.Type == IniType.Section)
									break;

								if (r.Type == IniType.Key)
									winEvents[r.Name] = r.Value;
							}
						} else
							r.MoveToNextSection();
					}
				}
			}
		}

		internal void Process()
		{
			Input.Update();

			InputProvider.Poll();
		}

		/// <summary>
		/// Saves the current control bindings to a path on the filesystem.
		/// </summary>
		/// <param name="filename">The location of the file to write to. NOTE: This does NOT automatically use Game.PathTo()</param>
		public void SaveConfiguration(String filename)
		{
			using (FileStream fs = new FileStream(filename, FileMode.Create)){
				using (IniWriter w = new IniWriter(fs))
				{
					w.WriteSection("KeyBindings");
					Command.commands.WriteIni(w);
				}
			}
		}

		/// <summary>
		/// Triggers a window event.
		/// </summary>
		/// <param name="eventname">The name of the window event to trigger.</param>
		public void WindowCommand(String eventname)
		{
			String cmd;
			if (winEvents.TryGetValue(eventname, out cmd))
				Command.TriggerCommand(new CommandData(cmd));
		}

		/// <summary>
		/// Tests if the input system has a command for a particular event.
		/// </summary>
		/// <param name="eventname">The event to test.</param>
		/// <returns>True if a command has been assigned, false otherwise.</returns>
		public bool HasWindowEvent(String eventname)
		{
			return winEvents.ContainsKey(eventname);
		}
	}
}
