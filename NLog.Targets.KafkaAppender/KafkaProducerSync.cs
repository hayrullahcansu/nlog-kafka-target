using Confluent.Kafka;
using NLog.Common;
using NLog.Targets.KafkaAppender.Configs;
using System;

namespace NLog.Targets.KafkaAppender
{
    public class KafkaProducerSync : KafkaProducerAbstract
    {
        public KafkaProducerSync(string brokers, KafkaProducerConfigs configs = null) : base(brokers, configs) { }

        public override void Produce(string topic, string data)
        {
            try
            {
                Producer.Produce(topic, new Message<Null, string>
                {
                    Value = data
                });
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, "KafkaAppender - Exception when sending message to topic={0}", topic);
                throw;
            }
        }
    }
}
