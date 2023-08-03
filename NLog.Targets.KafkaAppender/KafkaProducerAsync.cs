using System.Threading.Tasks;
using Confluent.Kafka;
using NLog.Targets.KafkaAppender.Configs;

namespace NLog.Targets.KafkaAppender
{
    public class KafkaProducerAsync : KafkaProducerAbstract
    {
        public KafkaProducerAsync(string brokers, KafkaProducerConfigs configs = null) : base(brokers, configs) { }

        public override void Produce(string topic, string data)
        {
            ProduceAsync(topic, data);
        }

        private async Task ProduceAsync(string topic, string data)
        {
            await Producer.ProduceAsync(topic, new Message<Null, string>
            {
                Value = data
            }).ConfigureAwait(false);
        }
    }
}
