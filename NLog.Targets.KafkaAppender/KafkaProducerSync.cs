using Confluent.Kafka;

namespace NLog.Targets.KafkaAppender
{
    public class KafkaProducerSync : KafkaProducerAbstract
    {
        public KafkaProducerSync(string brokers) : base(brokers) { }

        public override void Produce(string topic, string data)
        {
            Producer.Produce(topic, new Message<Null, string>
            {
                Value = data
            });
        }
    }
}
