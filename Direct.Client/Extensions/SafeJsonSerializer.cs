using System.Text.Json;
using Direct.Client.Helpers;
using Vostok.Logging.Abstractions;
using System;
using Direct.Client.Models;
using Direct.Client.Models.Errors;

namespace Direct.Client.Extensions
{
    public class SafeJsonSerializer
    {
        private ILog log;

        public SafeJsonSerializer(ILog log) 
        { 
            this.log = log;
        }
        public TValue SafeDeserialize<TValue>(string json, JsonSerializerOptions? options = null) where TValue : class
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

        public TValue TryDirectResponseDeserialize<TValue>(string body, string actionName) where TValue : class
        {
            TValue deserializeResult;
            try
            {
                deserializeResult = SafeDeserialize<TValue>(body);
            }
            catch (JsonException ex1)
            {
                try
                {
                    log.Warn(ex1.Message);
                    var directError = SafeDeserialize<DirectError<RequestError>>(body);                  
                    log.Warn($"{actionName}-FAILED Reason:{directError.error.error_string} Request_ID:{directError.error.request_id} Error_Code:{directError.error.error_code}");
                }
                catch (JsonException ex2)
                {
                    log.Warn($"{actionName}-FAILED Reason:{ex2.Message}");
                }
                return null;
            }
            return deserializeResult;
        }
    }
}
