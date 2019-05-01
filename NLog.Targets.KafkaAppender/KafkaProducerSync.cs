using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLog.Targets.KafkaAppender
{
    public class KafkaProducerSync : KafkaProducerAbstract
    {
        public KafkaProducerSync(string brokers) : base(brokers)
        {

        }

        public override void Produce(ref string topic, ref string data)
        {
            producer.Produce(topic, new Message<Confluent.Kafka.Null, string>()
            {
                Value = data
            });
        }
    }
}
