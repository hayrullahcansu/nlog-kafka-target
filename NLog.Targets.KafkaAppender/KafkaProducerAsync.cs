using System.Threading.Tasks;
using Confluent.Kafka;
using NLog.Targets.KafkaAppender.Configs;

namespace NLog.Targets.KafkaAppender
{
    public class KafkaProducerAsync : KafkaProducerAbstract
    {
        private TopicPartition _lastTopicPartition;

        public KafkaProducerAsync(string brokers, KafkaProducerConfigs configs = null) : base(brokers, configs) { }

        public override void Produce(string topic, string data)
        {
            var topicPartition = _lastTopicPartition;
            if (!string.Equals(topic, topicPartition?.Topic, System.StringComparison.Ordinal))
            {
                _lastTopicPartition = topicPartition = new TopicPartition(topic, Partition.Any);
            }
            ProduceAsync(topicPartition, data);
        }

        private Task ProduceAsync(TopicPartition topicPartition, string data)
        {
            return Producer.ProduceAsync(topicPartition, new Message<Null, string>
            {
                Value = data
            });
        }
    }
}
