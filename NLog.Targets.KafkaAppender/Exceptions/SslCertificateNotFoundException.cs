using System;

namespace NLog.Targets.KafkaAppender.Exceptions
{
    internal class SslCertificateNotFoundException : NLogConfigurationException
    {
        public SslCertificateNotFoundException() { }

        public SslCertificateNotFoundException(string message) : base(message) { }

        public SslCertificateNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
