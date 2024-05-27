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
            var conf = new ProducerConfig
            {
                BootstrapServers = brokers,
                SslCertificateLocation = configs?.SslCertificateLocation,
                SslCaLocation = configs?.SslCaLocation,
                SslKeyLocation = configs?.SslKeyLocation,
                SslKeyPassword = configs?.SslKeyPassword,
                SecurityProtocol = configs?.SecurityProtocol,
                MessageTimeoutMs = configs?.MessageTimeoutMs,
                SaslUsername = configs?.SaslUsername,
                SaslPassword = configs?.SaslPassword,
                SaslMechanism = configs?.SaslMechanism
            };
            if (!string.IsNullOrEmpty(configs?.ClientId))
            {
                conf.ClientId = configs.ClientId;
            }

            Producer = new ProducerBuilder<Null, string>(conf)
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