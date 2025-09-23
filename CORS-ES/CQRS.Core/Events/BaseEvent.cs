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
