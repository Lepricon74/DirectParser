using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Direct.ImageRecognitionClient.Helpers;
using Direct.ImageRecognitionClient.Interfaces;
using Direct.Shared.Models;

namespace Direct.ImageRecognitionClient.Services
{
	public class ImageRecognitionService
	{
		private IImageRecognitionUriProvider _uriProvider;
		private readonly ImageRecognitionRequestSender _imageRecognitionRequestSender;

		public ImageRecognitionService(IImageRecognitionUriProvider uriProvider, ImageRecognitionRequestSender imageRecognitionRequestSender)
		{
			_uriProvider = uriProvider;
			_imageRecognitionRequestSender = imageRecognitionRequestSender;
		}

		private Uri GetUriToImageTextRecognition()
			=> new Uri(_uriProvider.GetUri().AbsoluteUri + "imagesTextRecognition");

		public async Task<List<ImageRecognitionModel>> ImageToText(List<AdsImageModel> images)
			=> await _imageRecognitionRequestSender.SendImageRecognitionRequest(GetUriToImageTextRecognition, images, "GET-TEXT-FROM-IMAGES");
	}
}