using System;
using System.Runtime.Serialization;

namespace NLog.Targets.KafkaAppender.Exceptions
{
    [Serializable]
    public class BrokerNotFoundException : Exception
    {
        public BrokerNotFoundException() { }

        public BrokerNotFoundException(string message) : base(message) { }

        public BrokerNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        protected BrokerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
