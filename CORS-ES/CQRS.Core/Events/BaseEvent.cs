using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CQRS.Core.Messages;

namespace CQRS.Core.Events
{
    public abstract class BaseEvent : BaseMessage
    {
        public int Version { get; set; }
        public string Type { get; set; }

        protected BaseEvent(string type)
        {
            Type = type;
        }
    }
}
