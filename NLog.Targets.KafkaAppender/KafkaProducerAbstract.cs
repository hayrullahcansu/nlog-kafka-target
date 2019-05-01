using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLog.Targets.KafkaAppender
{
    public abstract class KafkaProducerAbstract : IDisposable
    {
        public KafkaProducerAbstract(string brokers)
        {
            var conf = new ProducerConfig
            {
                BootstrapServers = brokers
            };

            producer = new ProducerBuilder<Null, string>(conf).Build();
        }
        protected IProducer<Null, string> producer;

        public abstract void Produce(ref string topic, ref string data);

        public void Dispose()
        {
            producer?.Flush();
            producer?.Dispose();
        }
    }
}
