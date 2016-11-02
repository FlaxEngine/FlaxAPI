// Celelej Game Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CelelejEngine
{
    /// <summary>
    /// Base class for all content resources objects
    /// </summary>
    public abstract class Asset : Object
    {
        /// <summary>
        /// Gets asset name
        /// </summary>
        public string Name
        {
            get { return Internal_GetName(unmanagedPtr); }
        }
        
        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} ({1})", Internal_GetName(unmanagedPtr), GetType().Name);
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj);
        
        #endregion
    }
}
