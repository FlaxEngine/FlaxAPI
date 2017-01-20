using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    public partial class Asset
    {
        /// <inheritdoc />
        public override string ToString()
        {
            return $"{GetName} ({GetType().Name})";
        }
    }
}
