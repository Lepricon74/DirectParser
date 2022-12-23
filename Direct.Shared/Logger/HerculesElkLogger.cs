using System;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Hercules;
using Vostok.Hercules.Client;
using Vostok.Logging.Hercules.Configuration;
using Vostok.Clusterclient.Core.Topology;
using System.Collections.Generic;

namespace Direct.Shared.Logger
{
    public class HerculesElkLogger : ILog
    {
        private readonly ILog log;
        private readonly Dictionary<string, object> properties;

        public HerculesElkLogger(
            ILog failOverLog, 
            string apiKey, 
            IClusterProvider clusterProvider,
            string environment,
            string elkIndex,
            string project
            ) {
            Func<string> apiKeyProvider = () => { return apiKey;};
            var herculesSinkSetting = new HerculesSinkSettings(clusterProvider, apiKeyProvider);
            
            var herculesSink = new HerculesSink(herculesSinkSetting, failOverLog);
            var herculesLog = new HerculesLog(new HerculesLogSettings(herculesSink, "logs"));

            log = herculesLog;
            properties = new Dictionary<string, object> { { "environment", environment }, { "elk-index", elkIndex }, { "project", project }};
            failOverLog.Info("Hercules Log was created with properties - "
                             + "Environment : " + properties["environment"]+",  "
                             + "elk-index : " + properties["elk-index"]
                             + "project : " + properties["project"]);
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