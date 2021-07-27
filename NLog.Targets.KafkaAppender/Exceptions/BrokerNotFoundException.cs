using System;

namespace NLog.Targets.KafkaAppender.Exceptions
{
    public class BrokerNotFoundException : NLogConfigurationException
    {
        public BrokerNotFoundException() { }

        public BrokerNotFoundException(string message) : base(message) { }

        public BrokerNotFoundException(string message, Exception innerException) : base(message, innerException) { }

    }
}
