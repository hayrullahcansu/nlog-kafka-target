using Confluent.Kafka;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets.KafkaAppender.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

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

        private static KafkaProducerAbstract producer;
        private static readonly object _locker = new object();
        private static bool recovering = false;

        /// <summary>
        /// initializeTarget
        /// </summary>
        protected override void InitializeTarget()
        {
            base.InitializeTarget();
            try
            {
                if (Brokers == null || Brokers.Length == 0)
                {
                    throw new BrokerNotFoundException("Broker is not found");
                }
                if (producer == null)
                {
                    lock (_locker)
                    {
                        if (producer == null)
                        {
                            if (Async)
                            {
                                producer = new KafkaProducerAsync(Brokers);
                            }
                            else
                            {
                                producer = new KafkaProducerSync(Brokers);
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
                producer?.Dispose();
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
                string topic = this.Topic.Render(logEvent);
                string logMessage = this.Layout.Render(logEvent);
                producer.Produce(ref topic, ref logMessage);
            }
            catch (ProduceException<Null, string> ex)
            {
                if (ex.Error.IsFatal)
                {
                    if (!recovering)
                    {
                        lock (_locker)
                        {
                            if (!recovering)
                            {
                                recovering = true;
                                try
                                {
                                    producer?.Dispose();
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
                                    producer = new KafkaProducerAsync(Brokers);
                                }
                                else
                                {
                                    producer = new KafkaProducerSync(Brokers);
                                }
                                recovering = false;
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
            }

        }
    }
}
