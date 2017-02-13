using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEditor.History
{
    [Serializable]
    public class UndoActionObject : IHistoryAction
    {
        public Guid Id { get; }
        public string ActionString { get; }
        public object InstanceToRevert { get; }
    }
}
