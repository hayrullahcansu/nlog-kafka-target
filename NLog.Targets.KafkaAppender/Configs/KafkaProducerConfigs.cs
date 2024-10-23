using System.Collections.Generic;
using Confluent.Kafka;

namespace NLog.Targets.KafkaAppender.Configs
{
    public class KafkaProducerConfigs
    {
        /// <inheritdoc cref="ClientConfig.ClientId"/>
        public string ClientId { get; set; }
        /// <inheritdoc cref="ClientConfig.SslCertificateLocation"/>
        public string SslCertificateLocation { get; set; }
        /// <inheritdoc cref="ClientConfig.SslCaLocation"/>
        public string SslCaLocation { get; set; }
        /// <inheritdoc cref="ClientConfig.SslKeyLocation"/>
        public string SslKeyLocation { get; set; }
        /// <inheritdoc cref="ClientConfig.SslKeyPassword"/>
        public string SslKeyPassword { get; set; }
        /// <inheritdoc cref="ClientConfig.SecurityProtocol"/>
        public SecurityProtocol? SecurityProtocol { get; set; }
        /// <inheritdoc cref="ClientConfig.MessageTimeoutMs"/>
        public int? MessageTimeoutMs { get; set; }
        /// <inheritdoc cref="ClientConfig.SaslMechanism"/>
        public SaslMechanism? SaslMechanism { get; set; }
        /// <inheritdoc cref="ClientConfig.SaslUsername"/>
        public string SaslUsername { get; set; }
        /// <inheritdoc cref="ClientConfig.SaslPassword"/>
        public string SaslPassword { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}