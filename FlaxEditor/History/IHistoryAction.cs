using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEditor.History
{
    public interface IHistoryAction
    {
        /// <summary>
        /// Id of made action
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Name or key of performed action
        /// </summary>
        String ActionString { get; }
    }
}
