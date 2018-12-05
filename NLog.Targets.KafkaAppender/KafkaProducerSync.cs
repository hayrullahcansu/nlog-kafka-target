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

        public override void Produce(ref string topic, ref byte[] data)
        {
            producer.ProduceAsync(topic, null, 0, 0, data, 0, data.Length, 0).GetAwaiter().GetResult();
        }
    }
}
