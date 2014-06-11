using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.Core.Runtime
{
	public abstract class AbstractGameLoop
	{
		/// <summary>
		/// Called when the game is ready to begin. This is the entry-point for 
		/// subclasses of AbstractGameLoop.
		/// </summary>
		public abstract void Begin();
	}
}
