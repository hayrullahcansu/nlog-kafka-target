# nlog-kafka-target
nlog appender for kafka which provides the custom topics pattern and partitions

![Nuget](https://img.shields.io/nuget/dt/NLog.Targets.KafkaAppender)
[![GitHub issues](https://img.shields.io/github/issues/hayrullahcansu/nlog-kafka-target)](https://github.com/hayrullahcansu/nlog-kafka-target/issues)
![Nuget](https://img.shields.io/nuget/v/NLog.Targets.KafkaAppender)
[![GitHub forks](https://img.shields.io/github/forks/hayrullahcansu/nlog-kafka-target)](https://github.com/hayrullahcansu/nlog-kafka-target/network)
[![GitHub stars](https://img.shields.io/github/stars/hayrullahcansu/nlog-kafka-target)](https://github.com/hayrullahcansu/nlog-kafka-target/stargazers)


## Supported frameworks 
```
- .NET 5, 6 and 7
- .NET Core 2 and 3
- .NET Standard 2.0+
- .NET Framework 4.5 - 4.8
```

## Getting Started
### Step 1: Install NLog.Targets.KafkaAppender package from [nuget.org](https://www.nuget.org/packages/NLog.Targets.KafkaAppender/)
```
Install via Package-Manager   Install-Package NLog.Targets.KafkaAppender
Install via .NET CLI          dotnet add package NLog.Targets.KafkaAppender
```
### Step 2: Configure nlog sections

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true">
  <extensions>
    <add assembly="NLog.Targets.KafkaAppender"/>
  </extensions>
  <targets>
    <target xsi:type="KafkaAppender"
            name="kafka"
            topic="${callsite:className=true:fileName=false:includeSourcePath=false:methodName=true}"
            layout="${longdate}|${level:uppercase=true}|${logger}|${message}"
            brokers="localhost:9092"
            async="false"
            sslCertificateLocation=""
            securityProtocol="plaintext"
            >

    </target>
  </targets>
  <rules>
    <logger name="*" minlevel="Info" writeTo="kafka" />
  </rules>
</nlog>
```
| Param Name              | Variable Type | Requirement | Description                                                       | Default                                                                             | Possible values                            |
|-------------------------|---------------|-------------|-------------------------------------------------------------------|-------------------------------------------------------------------------------------|--------------------------------------------|
| name                    | `:string`     |    yes`*`   | Target's name                                                     |                                                                                     |                                            |
| topic                   | `:layout`     |    yes`*`   | Topic pattern can be layout                                       | `${logger}`                                                                         |                                            |
| layout                  | `:layout`     |      no     | Layout used to format log messages.                               | `${longdate}|${level:uppercase=true}|${logger}|${message}`                          |                                            |
| brokers                 | `:string`     |    yes`*`   | Kafka brokers with comma-separated                                |                                                                                     |                                            |
| async                   | `:boolean`    |      no     | Async or sync mode                                                | `false`                                                                             |                                            |
| sslCertificateLocation  | `:string`     |      no     | Path to ssl certificate                                           |                                                                                     |                                            |
| securityProtocol        | `:enum`       |      no     | Protocol used to communicate with brokers                         | `plaintext`                                                                         | `Plaintext` `Ssl` `SaslPlaintext` `SaslSsl`|
| messageTimeoutMs        | `:int`        |      no     | Limits the time a produced message waits for successful delivery  |                                                                                     |                                            |


Check documentation about all [`Layout Renderers`](https://nlog-project.org/config/?tab=layout-renderers)


## Usage

```cs
using System;

namespace NLog.Targets.KafkaAppender.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Error("hello world");
            
            //topic layout:     ${callsite:className=true:fileName=false:includeSourcePath=false:methodName=true}
            //message layout:   ${longdate}|${level:uppercase=true}|${logger}|${message}
            
            //topic output:     NLog.Targets.KafkaAppender.Test.Program.Main
            //message output:   2018-12-05 18:27:46.7382|ERROR|NLog.Targets.KafkaAppender.Test.Program|hello world 
            
        }
    }
}

```
