using Confluent.Kafka;
using NLog.Targets.KafkaAppender.Configs;

namespace NLog.Targets.KafkaAppender
{
    public class KafkaProducerSync : KafkaProducerAbstract
    {
        public KafkaProducerSync(string brokers, KafkaProducerConfigs configs = null) : base(brokers, configs) { }

        public override void Produce(string topic, string data)
        {
            Producer.Produce(topic, new Message<Null, string>
            {
                Value = data
            });
        }
    }
}
