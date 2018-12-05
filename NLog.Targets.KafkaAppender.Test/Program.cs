using System;

namespace NLog.Targets.KafkaAppender.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error("hello world");
        }
    }
}
