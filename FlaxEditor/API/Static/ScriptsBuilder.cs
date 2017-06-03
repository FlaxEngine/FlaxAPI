////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEditor.Scripting
{
	/// <summary>
	/// Game scrips building service. Compiles user C# scripts into binary assemblies.
	/// </summary>
	public static partial class ScriptsBuilder
	{
	    /// <summary>
	    /// Checks if need to compile source code. If so calls compilation.
	    /// </summary>
        public static void CheckForCompile()
	    {
	        if (IsSourceDirty)
	            Compile();
	    }
    }
}