using System.Collections.Generic;
using System.Threading.Tasks;
using Direct.ImageRecognitionClient.Services;
using Direct.Shared.Models;

namespace Direct.ImageRecognitionClient
{
	public class ImageToTextRecognitionClient
	{
		private ImageRecognitionService _imageRecognitionService;

		public ImageToTextRecognitionClient(ImageRecognitionService imageRecognitionService)
		{
			_imageRecognitionService = imageRecognitionService;
		}

		public async Task<List<ImageRecognitionModel>> ImagesToText(List<AdsImageModel> images) 
			=> await _imageRecognitionService.ImageToText(images);
	}
}

