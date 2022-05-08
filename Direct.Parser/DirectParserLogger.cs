using Vostok.Logging.Abstractions;
using Vostok.Logging.File;
using Vostok.Logging.Console;
using Vostok.Logging.File.Configuration;

namespace Direct.Parser
{
    internal static class DirectParserLogger
    {
        public static ILog Create() { 
            return new CompositeLog(
                    new FileLog(new FileLogSettings()),
                    new ConsoleLog()
                );
        }
    }
}
