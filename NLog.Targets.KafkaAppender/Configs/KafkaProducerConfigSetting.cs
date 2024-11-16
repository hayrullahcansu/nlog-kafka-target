using System.ComponentModel;
using NLog.Config;
using NLog.Layouts;

namespace NLog.Targets.KafkaAppender.Configs
{
    /// <summary>
    /// Kafka Producer Setting (Key-Value Pair). Ex. client.id = NLog_${machinename}
    /// </summary>
    /// <remarks>
    /// See also <see href="https://kafka.apache.org/documentation/#producerconfigs" />
    /// </remarks>
    [NLogConfigurationItem]
    public class KafkaProducerConfigSetting
    {
        public string Key { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Name { get => Key; set => Key = value; }
        public Layout Value { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Layout Layout { get => Value; set => Value = value; }
    }
}
