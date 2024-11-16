using Confluent.Kafka;
using NLog.Common;
using NLog.Targets.KafkaAppender.Configs;
using System;

namespace NLog.Targets.KafkaAppender
{
    public abstract class KafkaProducerAbstract : IDisposable
    {
        private bool _disposed;

        protected KafkaProducerAbstract(string brokers, KafkaProducerConfigs configs = null)
        {
            var producerConfig = configs?.Settings?.Count > 0 ? new ProducerConfig(configs.Settings) : new ProducerConfig();
            if (!string.IsNullOrEmpty(brokers))
            {
                producerConfig.BootstrapServers = brokers;
            }

            if (configs != null)
            {
                if (configs.SaslMechanism.HasValue)
                    producerConfig.SaslMechanism = configs.SaslMechanism;
                if (configs.SecurityProtocol.HasValue)
                    producerConfig.SecurityProtocol = configs.SecurityProtocol;
                if (configs.SslCertificateLocation != null)
                    producerConfig.SslCertificateLocation = configs.SslCertificateLocation;
                if (configs.SslCaLocation != null)
                    producerConfig.SslCaLocation = configs.SslCaLocation;
                if (configs.SslKeyLocation != null)
                    producerConfig.SslKeyLocation = configs.SslKeyLocation;
                if (configs.SslKeyPassword != null)
                    producerConfig.SslKeyPassword = configs.SslKeyPassword;
                if (configs.MessageTimeoutMs.HasValue)
                    producerConfig.MessageTimeoutMs = configs.MessageTimeoutMs;
                if (configs.SaslUsername != null)
                    producerConfig.SaslUsername = configs.SaslUsername;
                if (configs.SaslPassword != null)
                    producerConfig.SaslPassword = configs.SaslPassword;
                if (!string.IsNullOrEmpty(configs.ClientId))
                    producerConfig.ClientId = configs.ClientId;
            }

            Producer = new ProducerBuilder<Null, string>(producerConfig)
                .SetErrorHandler((producer, error) =>
                {
                    InternalLogger.Error("KafkaAppender - {0}Error when sending message to topic. ErrorCode={1}, Reason={2}", error.IsFatal ? "Fatal " : "", error.Code, error.Reason);
                })
                .Build();
        }

        protected IProducer<Null, string> Producer;

        public abstract void Produce(string topic, string data);

        public void Flush()
        {
            Producer?.Flush();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    Producer?.Flush();
                }
                finally
                {
                    Producer?.Dispose();
                    _disposed = true;
                }
            }
        }
    }
}