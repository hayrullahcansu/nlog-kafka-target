using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLog.Targets.KafkaAppender
{
    public class KafkaProducerAsync : KafkaProducerAbstract
    {
        public KafkaProducerAsync(string brokers) : base(brokers)
        {
        }

        public override void Produce(ref string topic, ref string data)
        {
            producer.ProduceAsync(topic, new Message<Confluent.Kafka.Null, string>()
            {
                Value = data
            });
        }
    }
}
