using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Sharplike.Storylib.DialogueTree
{
	[Serializable]
	public class StoryPhase
	{
		internal StoryPhase()
		{
		}

		public IList<DialogueOption> DialogueOptions
		{
			get
			{
				return dopts.AsReadOnly();
			}
		}

		public String Text
		{
			get;
			set;
		}

		public bool Conclusion
		{
			get
			{
				return dopts.Count > 0 ? false : true;
			}
		}

		public DialogueOption CreateDialogueOption(StoryPhase target)
		{
			DialogueOption opt = new DialogueOption();
			opt.Target = target;
			dopts.Add(opt);
			return opt;
		}

		private List<DialogueOption> dopts = new List<DialogueOption>();
	}
}
