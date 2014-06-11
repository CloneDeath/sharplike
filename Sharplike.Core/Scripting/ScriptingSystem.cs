using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Mono.Addins;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Sharplike.Core.Scripting
{
	public sealed class ScriptingSystem
	{
		internal ScriptingSystem()
		{
			//AddinManager.AddExtensionNodeHandler("/Sharplike/Scripting", ScriptsChanged);
			foreach (TypeExtensionNode node in AddinManager.GetExtensionNodes("/Sharplike/Scripting"))
			{
				IScriptingEngine eng = (IScriptingEngine)node.CreateInstance();
				engines.Add(node.Id, eng.Engine);
			}
		}

		void ScriptsChanged(object s, ExtensionNodeEventArgs args)
		{
			IScriptingEngine eng = (IScriptingEngine)args.ExtensionObject;

			switch (args.Change)
			{
				case ExtensionChange.Add:
					engines.Add(args.ExtensionNode.Id, eng.Engine);
					break;
				case ExtensionChange.Remove:
					engines.Remove(args.ExtensionNode.Id);
					break;
			}
		}

		public void Run(String file)
		{
			String ext = Path.GetExtension(file);
			engines[ext].ExecuteFile(file);
		}

		private Dictionary<String, ScriptEngine> engines = new Dictionary<string, ScriptEngine>();
	}
}
