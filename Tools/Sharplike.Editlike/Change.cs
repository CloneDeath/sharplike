// Type: UndoStack.Change
// Assembly: UndoStack, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BAB7A541-143C-42DB-9F59-14CFE17ADF3B
// Assembly location: C:\Projects\RogueLike\SharpLike\Externals\UndoStack\UndoStack.dll

using System;

namespace UndoStack
{
	public class Change
	{
		public object UserData { get; set; }

		public UndoStack Owner { get; internal set; }

		public event EventHandler<EventArgs> OnInvalidated;

		public event EventHandler<EventArgs> OnUndo;

		public event EventHandler<EventArgs> OnRedo;

		internal void Invalidate()
		{
			if (this.OnInvalidated == null)
				return;
			this.OnInvalidated((object)this, new EventArgs());
		}

		internal void Undo()
		{
			if (this.OnUndo == null)
				return;
			this.OnUndo((object)this, new EventArgs());
		}

		internal void Redo()
		{
			if (this.OnRedo == null)
				return;
			this.OnRedo((object)this, new EventArgs());
		}
	}
}
