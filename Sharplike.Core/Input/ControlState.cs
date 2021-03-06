﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nini.Ini;

namespace Sharplike.Core.Input
{
	/// <summary>
	/// A tree representing a list of valid commands.
	/// Commands are inherited from parent CommandControls.
	/// </summary>
	public class CommandControls
	{
		private CommandControls parent = null;
		private Dictionary<Keys, String> keycommands = new Dictionary<Keys, string>();
		private Dictionary<String, CommandControls> children = new Dictionary<string, CommandControls>();

		internal CommandControls(CommandControls parent = null)
		{
			this.parent = parent;
		}

		public CommandData GetCommand(Keys keypress, String state)
		{
			if (String.IsNullOrEmpty(state))
			{
				if (keycommands.ContainsKey(keypress))
				{
					CommandData cmd = new CommandData(keycommands[keypress]);
					return cmd;
				}
				else
				{
					return null;
				}
			}

			int dotindex = state.IndexOf('.');
			String childname = null;
			String childns = null;

			if (dotindex == -1) {
				childname = state;
			} else {
				childname = state.Substring(0, dotindex);
				childns = state.Substring(dotindex + 1);
			}

			CommandData childresult = null;
			if (children.ContainsKey(childname)) {
				childresult = children[childname].GetCommand(keypress, childns);
			}

			if (childresult == null)
			{
				if (keycommands.ContainsKey(keypress))
				{
					CommandData cmd = new CommandData(keycommands[keypress]);
					return cmd;
				}
				else
				{
					return null;
				}
			}
			return childresult;
		}

		public void SetCommand(Keys keycode, String command, String statename)
		{
			CommandControls cs = GetChild(statename, true);
			if (cs == null)
				keycommands[keycode] = command;
			cs.keycommands[keycode] = command;
		}

		private String GetCommand(Keys keypress)
		{
			if (!keycommands.ContainsKey(keypress))
			{
				if (parent != null)
					return parent.GetCommand(keypress);
				return null;
			}
			return keycommands[keypress];
		}

		internal CommandControls GetChild(String location, bool create)
		{
			if (location == null)
				return this;

			int dotindex = location.IndexOf('.');
			String childname = null;
			String childns = null;
			if (dotindex == -1)
				childname = location;
			else
			{
				childname = location.Substring(0, dotindex);
				childns = location.Substring(dotindex + 1);
			}

			if (!children.ContainsKey(childname) && create)
			{
				CommandControls cs = new CommandControls(this);
				children[childname] = cs;
			}

			if (children.ContainsKey(childname))
				return children[childname].GetChild(childns, create);
			else
				return null;
		}

		#region Save/Load
		public void WriteIni(IniWriter w)
		{
			WriteIni(w, null);
		}
		private void WriteIni(IniWriter w, String ownns)
		{
			foreach (KeyValuePair<Keys, String> kvp in keycommands)
			{
				w.WriteKey(kvp.Key.ToString(), kvp.Value);
			}

			w.WriteEmpty();

			foreach (KeyValuePair<String, CommandControls> kvp in children)
			{
				String childns = kvp.Key;
				if (ownns != null)
					childns = String.Format("{0}.{1}", ownns, kvp.Key);

				w.WriteSection(String.Format("KeyBindings {0}", childns));
				kvp.Value.WriteIni(w, childns);
			}
		}

		public void ReadIni(IniReader r)
		{
			while (r.MoveToNextKey())
				keycommands.Add((Keys)Enum.Parse(typeof(Keys), r.Name), r.Value);
		}
		#endregion		
	}
}
