using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Direct.Shared.Common;
using Direct.Shared.Extensions;
using Direct.Shared.Models;
using Vostok.Logging.Abstractions;

namespace Direct.ImageRecognitionClient.Helpers
{
	public class ImageRecognitionRequestSender
	{
		private const string LOG_PREFIX = "\t";

		private ILog _log;
		private HttpClient _httpClient;

		public ImageRecognitionRequestSender(
			ILog log,
			HttpClient httpClient)
		{
			_log = log;
			_httpClient = httpClient;
		}

		public async Task<List<ImageRecognitionModel>> SendImageRecognitionRequest(Func<Uri> getUri, List<AdsImageModel> images, string actionName)
		{
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = getUri(),
			};
			var jsonRequestContent = JsonSerializer.Serialize(new { images });
			request.Content = new StringContent(jsonRequestContent, Encoding.UTF8, "application/json");
			_log.Info($"START-REQUEST-{actionName}\n" +
					 $"{LOG_PREFIX}Uri: {request.RequestUri}\n" +
					 $"{LOG_PREFIX}RequestBody: {Converter.ConvertJsonToStringForPrint(jsonRequestContent, $"{LOG_PREFIX}")}");

			var result = await _httpClient.SafeSendAsync(request, _log);
			if (result == null)
			{
				_log.Info($"{actionName}-FAILED Uri:{request.RequestUri}");
				return default;
			}

			var body = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
			var deserializeResult = JsonSerializer.Deserialize<List<ImageRecognitionModel>>(body);
			return deserializeResult;
		}
	}
}