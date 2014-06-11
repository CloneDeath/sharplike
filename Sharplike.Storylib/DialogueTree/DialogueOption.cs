using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Sharplike.Storylib.DialogueTree
{
	[Serializable]
	public class DialogueOption
	{
		public String Summary
		{
			get;
			set;
		}

		public StoryPhase Target
		{
			get;
			set;
		}

		internal void OnOptionChosen()
		{
			if (OptionChosen != null)
				OptionChosen(this, new EventArgs());
		}

		public event EventHandler<EventArgs> OptionChosen;
	}
}
