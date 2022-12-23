using System;
using Direct.ImageRecognitionClient.Interfaces;

namespace Direct.ImageRecognitionClient.Providers
{
	public class ImageRecognitionApiUrlProvider : IImageRecognitionUriProvider
	{
		private readonly Uri _uri;
		public ImageRecognitionApiUrlProvider(Uri uri) => _uri = uri;
		public Uri GetUri() => _uri;
	}
}