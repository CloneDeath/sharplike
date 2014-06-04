// Type: UndoStack.UndoStack
// Assembly: UndoStack, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BAB7A541-143C-42DB-9F59-14CFE17ADF3B
// Assembly location: C:\Projects\RogueLike\SharpLike\Externals\UndoStack\UndoStack.dll

using System;
using System.Collections.Generic;

namespace UndoStack
{
	public class UndoStack : IDisposable
	{
		private uint stackDepth;
		private int stackLevel;
		private List<Change> changes;

		public uint StackDepth
		{
			get
			{
				return this.stackDepth;
			}
			set
			{
				this.stackDepth = value;
				if ((long)this.changes.Count <= (long)this.stackDepth)
					return;
				List<Change> range = this.changes.GetRange((int)this.stackDepth + 1, this.changes.Count - (int)this.stackDepth);
				this.changes.RemoveRange((int)this.stackDepth + 1, this.changes.Count - (int)this.stackDepth);
				foreach (Change change in range)
					change.Invalidate();
			}
		}

		public UndoStack()
		{
			this.changes = new List<Change>();
			this.stackLevel = 0;
			this.StackDepth = 50U;
		}

		public void Dispose()
		{
			foreach (Change change in this.changes)
				change.Invalidate();
		}

		public void Undo()
		{
			if (this.stackLevel == this.changes.Count)
				return;
			this.changes[this.stackLevel++].Undo();
		}

		public void Redo()
		{
			if (this.stackLevel == 0)
				return;
			this.changes[--this.stackLevel].Redo();
		}

		public Change AddChange(Change newChange)
		{
			if (this.stackLevel > 0) {
				List<Change> range = this.changes.GetRange(0, this.stackLevel);
				this.changes.RemoveRange(0, this.stackLevel);
				foreach (Change change in range)
					change.Invalidate();
			}
			newChange.Owner = this;
			this.changes.Insert(0, newChange);
			this.stackLevel = 0;
			if ((long)this.changes.Count > (long)this.StackDepth) {
				Change change = this.changes[this.changes.Count - 1];
				change.Invalidate();
				this.changes.Remove(change);
			}
			return newChange;
		}

		public Change AddChange()
		{
			return this.AddChange(new Change());
		}
	}
}
