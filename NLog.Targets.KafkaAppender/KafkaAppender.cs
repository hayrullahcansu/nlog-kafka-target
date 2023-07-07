using Confluent.Kafka;
using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets.KafkaAppender.Configs;
using NLog.Targets.KafkaAppender.Exceptions;
using System;
using System.ComponentModel;
using System.IO;

namespace NLog.Targets.KafkaAppender
{
    [Target("KafkaAppender")]
    public class KafkaAppender : TargetWithLayout
    {
        /// <summary>
        /// Gets or sets the layout used to format topic of log messages.
        /// </summary>
        /// <remarks>
        /// Kafka topic has max length of 255, and allows the characters: a-z, A-Z, 0-9, . (dot), _ (underscore), and - (dash).
        /// </remarks>
        [RequiredParameter]
        [DefaultValue("${logger}")]
        public Layout Topic { get; set; }

        /// <summary>
        /// Kafka brokers with comma-separated
        /// </summary>
        [RequiredParameter]
        public Layout Brokers { get; set; }

        /// <summary>
        /// Path to certificate (client's public key - PEM) used for authentication.
        /// </summary>
        public Layout SslCertificateLocation { get; set; }

        /// <summary>
        /// Protocol used to communicate with brokers.
        /// </summary>
        public SecurityProtocol? SecurityProtocol { get; set; }

        /// <summary>
        /// Gets or sets async or sync mode
        /// </summary>
        public bool Async { get; set; } = false;

        private KafkaProducerAbstract _producer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaAppender"/> class.
        /// </summary>
        public KafkaAppender()
        {
            OptimizeBufferReuse = true;
        }

        /// <summary>
        /// initializeTarget
        /// </summary>
        protected override void InitializeTarget()
        {
            if (_producer == null)
            {
                InitializeKafkaProducer();
            }
            
            base.InitializeTarget();
        }

        private void InitializeKafkaProducer()
        {
            var brokers = RenderLogEvent(Brokers, LogEventInfo.CreateNullEvent());
            if (string.IsNullOrEmpty(brokers))
            {
                throw new BrokerNotFoundException("Broker is not found");
            }

            var sslCertificateLocation = RenderLogEvent(SslCertificateLocation, LogEventInfo.CreateNullEvent());
            if (!string.IsNullOrEmpty(sslCertificateLocation) && !File.Exists(sslCertificateLocation))
            {
                throw new SslCertificateNotFoundException($"Could not find certificate by specified path: {sslCertificateLocation}");
            }

            var configs = new KafkaProducerConfigs
            {
                SslCertificateLocation = string.IsNullOrEmpty(sslCertificateLocation) ? null : sslCertificateLocation,
                SecurityProtocol = SecurityProtocol,
            };

            try
            {
                _producer?.Dispose();
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, "KafkaAppender(Name={0}) - Exception when disposing producer during recovery.", Name);
            }

            try
            {
                if (Async)
                {
                    _producer = new KafkaProducerAsync(brokers, configs);
                }
                else
                {
                    _producer = new KafkaProducerSync(brokers, configs);
                }
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, "KafkaAppender(Name={0}) - Failed creating producer with Kafka-Brokers: {1}", Name, brokers);
                throw;
            }
        }

        /// <summary>
        /// disposing the target
        /// </summary>
        protected override void CloseTarget()
        {
            try
            {
                _producer?.Dispose();
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, "KafkaAppender(Name={0}) - Exception when disposing producer during close.", Name);
                throw;
            }
            finally
            {
                _producer = null;   // Let go of disposed producer
            }

            base.CloseTarget();
        }

        /// <summary>
        /// log event will be appended over broker
        /// </summary>
        /// <param name="logEvent"></param>
        protected override void Write(LogEventInfo logEvent)
        {
            var topic = RenderLogEvent(Topic, logEvent);
            var logMessage = RenderLogEvent(Layout, logEvent);

            try
            {
                _producer.Produce(topic, logMessage);
            }
            catch (ProduceException<Null, string> ex)
            {
                InternalLogger.Warn(ex, "KafkaAppender(Name={0}) - {1}Exception when sending message to topic={2}. Reason={3}", Name, ex.Error.IsFatal ? "Fatal " : "", topic, ex.Error.ToString());

                if (ex.Error.IsFatal)
                {
                    InitializeKafkaProducer();
                }
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, "KafkaAppender(Name={0}) - Exception when sending message to topic={1}", Name, topic);
                throw;
            }
        }

        /// <summary>
        /// flushing the target
        /// </summary>
        protected override void FlushAsync(AsyncContinuation asyncContinuation)
        {
            try
            {
                _producer?.Flush();
                asyncContinuation(null);
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, "KafkaAppender(Name={0}) - Exception when flushing producer.", Name);
                asyncContinuation(ex);
            }
        }
    }
}
