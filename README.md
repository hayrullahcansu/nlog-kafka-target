# nlog-kafka-target
nlog appender for kafka which provides the custom topics pattern and partitions

## Supported frameworks 
```
.NET Framework 3.5, 4, 4.5, 4.6 & 4.7
.NET Framework 4 client profile
ASP.NET 4 (NLog.Web package)
ASP.NET Core (NLog.Web.AspNetCore package)
.NET Core (NLog.Extensions.Logging package)
.NET Standard 1.3+ - NLog 4.5
.NET Standard 2.x+ - NLog 4.5
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
            debug="false"
            async="false"
            >

    </target>
  </targets>
  <rules>
    <logger name="*" minlevel="Info" writeTo="kafka" />
  </rules>
</nlog>
```
| Param Name | Variable Type | Requirement | Description                         | Default                                                                             |
|------------|---------------|-------------|-------------------------------------|-------------------------------------------------------------------------------------|
| name       | `:string`     |    yes`*`   | Target's name                       |                                                                                     |
| topic      | `:layout`     |    yes`*`   | Topic pattern can be layout         | `${callsite:className=true:fileName=false:includeSourcePath=false:methodName=true}` |
| layout     | `:layout`     |      no     | Layout used to format log messages. | `${longdate}|${level:uppercase=true}|${logger}|${message}`                          |
| brokers    | `:string`     |    yes`*`   | Kafka brokers with comma-separated  |                                                                                     |
| debug      | `:boolean`    |      no     | Debugging mode enabled              | `false`                                                                             |
| async      | `:boolean`    |      no     | Async or sync mode                  | `false`                                                                             |


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
