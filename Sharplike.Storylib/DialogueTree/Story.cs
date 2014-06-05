using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sharplike.Storylib.DialogueTree
{
    [Serializable]
    public class Story
    {
        public Story()
        {
            Beginning = CreatePhase();
        }

        public StoryPhase CreatePhase()
        {
            StoryPhase phase = new StoryPhase();
            phases.Add(phase);
            return phase;
        }

        public StoryPhase Beginning
        {
            get;
            set;
        }

        public StoryVisitor GetVisitor()
        {
            return new StoryVisitor(Beginning);
        }

        private List<StoryPhase> phases = new List<StoryPhase>();
    }
}
