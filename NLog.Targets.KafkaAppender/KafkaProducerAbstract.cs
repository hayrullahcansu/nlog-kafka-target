using Confluent.Kafka;
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
                SecurityProtocol = configs?.SecurityProtocol,
                MessageTimeoutMs = configs?.MessageTimeoutMs
            };

            Producer = new ProducerBuilder<Null, string>(conf).Build();
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