using System.Text.Json;
using Direct.Client.Helpers;
using Vostok.Logging.Abstractions;
using System;
using Direct.Client.Models;
using Direct.Client.Models.Errors;

namespace Direct.Client.Extensions
{
    public class SafeJsonResponseDeserializer
    {
        private ILog log;

        public SafeJsonResponseDeserializer(ILog log) 
        { 
            this.log = log;
        }
#nullable enable
        private ResponseType SafeDeserialize<ResponseType>(string json, JsonSerializerOptions? options = null) where ResponseType : class
        {
            var deserializeResult = JsonSerializer.Deserialize<ResponseType>(json, options);
            try
            {
                JsonValidator.NullCheck(deserializeResult);
            }
            catch (ArgumentNullException ex) {
                log.Warn(ex.Message);
                throw new JsonException();
            }         
            return deserializeResult;
        }

        public DirectResponse<ResponseResultType> TryDirectResponseDeserialize<ResponseResultType>(string body, string actionName) where ResponseResultType : class
        {
            DirectResponse<ResponseResultType> deserializeResult;
            try
            {
                deserializeResult = SafeDeserialize<DirectResponse<ResponseResultType>>(body);
                log.Info($"JSON Deserialize for {typeof(ResponseResultType).Name} success");
            }
            catch (JsonException resultDeserializeEx)
            {
                try
                {
                    log.Warn($"JSON Deserialize for {typeof(ResponseResultType).GetType().Name} failed");
                    var directError = SafeDeserialize<DirectError<RequestError>>(body);                  
                    log.Warn($"{actionName}-FAILED Reason:{directError.error.error_string} Request_ID:{directError.error.request_id} Error_Code:{directError.error.error_code}");
                }
                catch (JsonException errorDeserializeEx)
                {
                    log.Warn($"{actionName}-FAILED Reason:{errorDeserializeEx.Message}");
                }
                return null;
            }
            return deserializeResult;
        }
    }
}
