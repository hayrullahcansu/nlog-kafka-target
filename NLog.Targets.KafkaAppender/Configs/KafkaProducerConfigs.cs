using Confluent.Kafka;

namespace NLog.Targets.KafkaAppender.Configs
{
    public class KafkaProducerConfigs
    {
        public string SslCertificateLocation{ get; set; }
        public SecurityProtocol? SecurityProtocol { get; set; }
    }
}
