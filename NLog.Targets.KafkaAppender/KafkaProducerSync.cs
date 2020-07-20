using Confluent.Kafka;

namespace NLog.Targets.KafkaAppender
{
    public class KafkaProducerSync : KafkaProducerAbstract
    {
        public KafkaProducerSync(string brokers) : base(brokers) { }

        public override void Produce(ref string topic, ref string data)
        {
            Producer.Produce(topic, new Message<Null, string>
            {
                Value = data
            });
        }
    }
}
