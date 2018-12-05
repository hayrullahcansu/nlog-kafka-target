using System;
using System.Collections.Generic;
using System.Text;

namespace NLog.Targets.KafkaAppender.Exceptions
{
    public class BrokerNotFoundException : Exception
    {
        public BrokerNotFoundException(string message) : base(message)
        {
        }
    }
}
