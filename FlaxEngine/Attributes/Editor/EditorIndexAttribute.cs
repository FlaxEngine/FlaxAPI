using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    // TODO expand with insert after and before

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Event | AttributeTargets.Method)]
    public sealed class EditorIndexAttribute : Attribute
    {
        /// <summary>
        /// Requested index to perform layout on
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Override display index in visual tree for provided model
        /// </summary>
        /// <remarks>
        /// Current index is resolved runtime, and can change if custom editor class has changed.
        /// </remarks>
        /// <param name="index">Index in </param>
        public EditorIndexAttribute(int index)
        {
            Index = index;
        }
    }


}