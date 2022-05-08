using System.Text.Json;
using Direct.Client.Helpers;
using Vostok.Logging.Abstractions;
using System;

namespace Direct.Client.Extensions
{
    public class SafeJsonSerializer
    {
        private ILog log;

        public SafeJsonSerializer(ILog log) 
        { 
            this.log = log;
        }
        public TValue Deserialize<TValue>(string json, JsonSerializerOptions? options = null) where TValue : class
        {
            var deserializeResult = JsonSerializer.Deserialize<TValue>(json, options);
            try
            {
                JsonValidator.NullCheck(deserializeResult);
            }
            catch (ArgumentNullException ex) {
                log.Warn(ex.Message);
                throw new JsonException($"JSON Deserialize for {deserializeResult.GetType().Name} failed");
            }
            log.Info($"JSON Deserialize for {deserializeResult.GetType().Name} success");
            return deserializeResult;
        }
    }
}
