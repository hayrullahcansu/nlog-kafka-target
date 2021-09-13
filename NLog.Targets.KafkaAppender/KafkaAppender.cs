using Confluent.Kafka;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets.KafkaAppender.Exceptions;
using System;
using System.ComponentModel;

namespace NLog.Targets.KafkaAppender
{
    [Target("KafkaAppender")]
    public class KafkaAppender : TargetWithLayout
    {
        /// <summary>
        /// Gets or sets the layout used to format topic of log messages.
        /// </summary>
        [RequiredParameter]
        [DefaultValue("${callsite:className=true:fileName=false:includeSourcePath=false:methodName=true}")]
        public Layout Topic { get; set; }

        /// <summary>
        /// Gets or sets the layout used to format log messages.
        /// </summary>
        [DefaultValue("${longdate}|${level:uppercase=true}|${logger}|${message}")]
        public override Layout Layout { get; set; }

        /// <summary>
        /// Kafka brokers with comma-separated
        /// </summary>
        [RequiredParameter]
        public string Brokers { get; set; }


        /// <summary>
        /// Gets or sets debugging mode enabled
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Gets or sets async or sync mode
        /// </summary>
        public bool Async { get; set; } = false;

        private KafkaProducerAbstract _producer;
        private readonly object _locker = new object();
        private bool _recovering;

        /// <summary>
        /// initializeTarget
        /// </summary>
        protected override void InitializeTarget()
        {
            base.InitializeTarget();
            try
            {
                if (string.IsNullOrEmpty(Brokers))
                {
                    throw new BrokerNotFoundException("Broker is not found");
                }
                if (_producer == null)
                {
                    lock (_locker)
                    {
                        if (_producer == null)
                        {
                            if (Async)
                            {
                                _producer = new KafkaProducerAsync(Brokers);
                            }
                            else
                            {
                                _producer = new KafkaProducerSync(Brokers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Debug)
                {
                    Console.WriteLine(ex.ToString());
                }
                base.CloseTarget();
            }
        }

        /// <summary>
        /// disposing the target
        /// </summary>
        protected override void CloseTarget()
        {
            base.CloseTarget();
            try
            {
                _producer?.Dispose();
                _producer = null;
            }
            catch (Exception ex)
            {
                if (Debug)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// log event will be appended over broker
        /// </summary>
        /// <param name="logEvent"></param>
        protected override void Write(LogEventInfo logEvent)
        {
            try
            {
                var topic = Topic.Render(logEvent);
                var logMessage = Layout.Render(logEvent);
                _producer.Produce(ref topic, ref logMessage);
            }
            catch (ProduceException<Null, string> ex)
            {
                if (ex.Error.IsFatal && !_recovering)
                {
                    lock (_locker)
                    {
                        if (!_recovering)
                        {
                            _recovering = true;
                            try
                            {
                                _producer?.Dispose();
                            }
                            catch (Exception ex2)
                            {
                                if (Debug)
                                {
                                    Console.WriteLine(ex2.ToString());
                                }
                            }
                            if (Async)
                            {
                                _producer = new KafkaProducerAsync(Brokers);
                            }
                            else
                            {
                                _producer = new KafkaProducerSync(Brokers);
                            }
                            _recovering = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Debug)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

        }
    }
}
