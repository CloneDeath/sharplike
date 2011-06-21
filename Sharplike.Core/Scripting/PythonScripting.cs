﻿///////////////////////////////////////////////////////////////////////////////
/// Sharplike, The Open Roguelike Library (C) 2010 Ed Ropple.               ///
///                                                                         ///
/// This code is part of the Sharplike Roguelike library, and is licensed   ///
/// under the Common Public Attribution License (CPAL), version 1.0. Use of ///
/// this code is purusant to this license. The CPAL grants you certain      ///
/// permissions and requirements and should be read carefully before using  ///
/// this library.                                                           ///
///                                                                         ///
/// A copy of this license can be found in the Sharplike root directory,    ///
/// and must be included with all projects released using this library.     ///
///////////////////////////////////////////////////////////////////////////////

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
