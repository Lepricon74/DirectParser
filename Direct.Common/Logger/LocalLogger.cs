using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Vostok.Logging.Abstractions;
using Vostok.Logging.File;
using Vostok.Logging.Console;
using System.Linq;
using Vostok.Logging.File.Configuration;

namespace Direct.Common.Logger
{
    public class LocalLogger : ILog, IDisposable
    {
        private readonly ILog log;

        private readonly IEnumerable<IDisposable> disposableLoggers;

        public LocalLogger() {
            disposableLoggers = new List<IDisposable>();
            var settingsForAllLogs = new FileLogSettings {
                RollingStrategy = new RollingStrategyOptions { Type = RollingStrategyType.ByTime },
                FilePath = "logs/{RollingSuffix}.log",
                FileOpenMode = FileOpenMode.Append,
                Encoding = Encoding.UTF8,
                FileShare = FileShare.ReadWrite,
                UseSeparateFileOnConflict = false,
            };
            var logForAllLogs = new FileLog(settingsForAllLogs);

            var settingsForErrorLogs = new FileLogSettings
            {
                RollingStrategy = new RollingStrategyOptions { Type = RollingStrategyType.ByTime },
                FilePath = "logs/{RollingSuffix}.errors.log",
                FileOpenMode = FileOpenMode.Append,
                Encoding = Encoding.UTF8,
                FileShare = FileShare.ReadWrite,
                EnabledLogLevels = new[] { LogLevel.Fatal, LogLevel.Warn, LogLevel.Error },
                UseSeparateFileOnConflict = false,
            };
            var logForErrorsLogs = new FileLog(settingsForErrorLogs);
            this.disposableLoggers = disposableLoggers.Append(logForAllLogs);
            this.disposableLoggers = disposableLoggers.Append(logForErrorsLogs);

            var consoleLog = new ConsoleLog();

            this.log = new CompositeLog(
                    consoleLog,
                    logForAllLogs,
                    logForErrorsLogs
                );
        }

        public void Log(LogEvent @event) { 
            this.log.Log(@event);
        }

        public bool IsEnabledFor(LogLevel level) { 
            return this.log.IsEnabledFor(level);
        }

        public ILog ForContext(string context) {
            return this.log.ForContext(context);
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose Logs");
            foreach (var log in this.disposableLoggers) { log.Dispose(); }
        }
    }
}
