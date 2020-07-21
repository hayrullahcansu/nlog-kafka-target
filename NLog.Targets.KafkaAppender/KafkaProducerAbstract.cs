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

        public abstract void Produce(ref string topic, ref string data);

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
                Producer?.Flush();
                Producer?.Dispose();
            }

            _disposed = true;
        }
    }
}