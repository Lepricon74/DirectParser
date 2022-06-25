using System;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Hercules;
using Vostok.Hercules.Client;
using Vostok.Logging.Hercules.Configuration;
using Vostok.Clusterclient.Core.Topology;
using System.Collections.Generic;

namespace Direct.Runner.Logger
{
    public class DirectRunnerHerculesElkLogger : ILog
    {
        private readonly ILog log;
        private readonly Dictionary<string, object> properties;

        public DirectRunnerHerculesElkLogger(
            ILog failOverLog, 
            string apiKey, 
            IClusterProvider clusterProvider,
            string environment,
            string elkIndex
            ) {
            Func<string> apiKeyProvider = () => { return apiKey;};
            var herculesSinkSetting = new HerculesSinkSettings(clusterProvider, apiKeyProvider);
            
            var herculesSink = new HerculesSink(herculesSinkSetting, failOverLog);
            var herculesLog = new HerculesLog(new HerculesLogSettings(herculesSink, "logs"));

            log = herculesLog;
            properties = new Dictionary<string, object> { { "environment", environment }, { "elk-index", elkIndex } };
            failOverLog.Info("Hercules Log was created with properties: " + properties.ToString());
        }

        public void Log(LogEvent @event)
        {
            var eventWithElkProps = new LogEvent(@event.Level, @event.Timestamp, @event.MessageTemplate, properties, null);
            this.log.Log(eventWithElkProps);
        }

        public bool IsEnabledFor(LogLevel level) { 
            return this.log.IsEnabledFor(level);
        }

        public ILog ForContext(string context) {
            return this.log.ForContext(context);
        }
    }
}