using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Sharplike.Core.Scripting
{
	public interface IScriptingEngine
	{
		ScriptEngine Engine
		{
			get;
		}
	}
}
