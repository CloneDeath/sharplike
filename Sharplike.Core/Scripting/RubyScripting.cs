using System;
using System.Collections.Generic;
using System.Text;
using IronRuby;
using IronRuby.Hosting;
using Microsoft.Scripting.Hosting;

namespace Sharplike.Core.Scripting
{
	public class RubyScripting : IScriptingEngine
	{
		public ScriptEngine Engine
		{
			get 
			{
				if (engine == null)
					engine = Ruby.CreateEngine();
				return engine; 
			}
		}

		private ScriptEngine engine;
	}
}
