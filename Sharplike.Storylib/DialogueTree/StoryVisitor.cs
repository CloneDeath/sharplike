using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Storylib.DialogueTree
{
	public class StoryVisitor
	{
		public StoryVisitor(StoryPhase begin)
		{
			current = begin;
		}

		public StoryPhase VisitNext(DialogueOption opt)
		{
			return VisitNext(current.DialogueOptions.IndexOf(opt));
		}

		public StoryPhase VisitNext(int index)
		{
			DialogueOption opt = current.DialogueOptions[index];
			opt.OnOptionChosen();
			current = opt.Target;
			return current;
		}

		private StoryPhase current;
	}
}
