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

        public override void Produce(ref string topic, ref byte[] data)
        {
            producer.ProduceAsync(topic, null, 0, 0, data, 0, data.Length, 0);
        }
    }
}
