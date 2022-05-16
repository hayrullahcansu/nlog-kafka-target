using Confluent.Kafka;
using System;

namespace NLog.Targets.KafkaAppender
{
    public abstract class KafkaProducerAbstract : IDisposable
    {
        private bool _disposed;

        protected KafkaProducerAbstract(string brokers)
        {
            var conf = new ProducerConfig
            {
                BootstrapServers = brokers
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