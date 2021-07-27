using Confluent.Kafka;

namespace NLog.Targets.KafkaAppender
{
    public class KafkaProducerAsync : KafkaProducerAbstract
    {
        public KafkaProducerAsync(string brokers) : base(brokers) { }

        public override void Produce(string topic, string data)
        {
            Producer.ProduceAsync(topic, new Message<Null, string>
            {
                Value = data
            });
        }
    }
}
