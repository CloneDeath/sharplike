using System;
using System.Collections.Generic;
using System.Text;
using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Sharplike.Core.Scripting
{
    public class PythonScripting : IScriptingEngine
    {
        public ScriptEngine Engine
        {
            get
            {
                if (engine == null)
                    engine = Python.CreateEngine();
                return engine;
            }
        }
        private ScriptEngine engine;
    }
}
