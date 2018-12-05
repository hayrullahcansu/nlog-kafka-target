using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLog.Targets.KafkaAppender
{
    public abstract class KafkaProducerAbstract:IDisposable
    {
        public KafkaProducerAbstract(string brokers)
        {
            var conf = new Dictionary<string, object>
                {
                  { "bootstrap.servers", brokers }
                };

            producer = new Producer(conf);
        }
        protected Producer producer;

        public abstract void Produce(ref string topic, ref byte[] data);

        public void Dispose()
        {
            producer?.Dispose();
        }
    }
}
