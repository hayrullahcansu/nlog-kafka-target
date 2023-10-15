using Confluent.Kafka;

namespace NLog.Targets.KafkaAppender.Configs
{
    public class KafkaProducerConfigs
    {
        public string SslCertificateLocation { get; set; }

        public string SslCaLocation { get; set; }

        public string SslKeyLocation { get; set; }

        public string SslKeyPassword { get; set; }

        public SecurityProtocol? SecurityProtocol { get; set; }

        public int? MessageTimeoutMs { get; set; }

    }
}
