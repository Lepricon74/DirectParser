using System;
using System.Threading.Tasks;
using Direct.Client.Helpers;
using Direct.Client.Interfaces;
using Direct.Client.Models;
using Direct.Client.Models.AdImages;

namespace Direct.Client.Services
{
	public class AdImagesService
	{
		private IUriProvider _uriProvider;
		private DirectRequestSender _directRequestSender;

		public AdImagesService(DirectRequestSender directRequestSender, IUriProvider uriProvider)
		{
			_directRequestSender = directRequestSender;
			_uriProvider = uriProvider;
		}

		private enum AvailableRequestFieldNames
		{
			AdImageHash,
			OriginalUrl
		}

		private Uri GetUriToAdImagesService() {
			return new Uri(_uriProvider.GetUri().AbsoluteUri + "/adimages");
		}

		public async Task<AdImagesResponseResult> GetAdImages(string[] adImageHashes, string associated = "YES")
		{
			var actionName = "GET-ALL-IMAGES";
			CommonRequestParams<AdImagesRequestSelectionCriteria> GetRequestParams()
			{
				return new CommonRequestParams<AdImagesRequestSelectionCriteria>(
					new AdImagesRequestSelectionCriteria(adImageHashes, associated),
					Enum.GetNames(typeof(AvailableRequestFieldNames))
					);
			}

			var imagesResponseResult = await _directRequestSender.SendDirectGetRequest<
				CommonRequestParams<AdImagesRequestSelectionCriteria>, AdImagesResponseResult>(
				GetRequestParams,
				GetUriToAdImagesService,
				actionName);
			return imagesResponseResult;
		}
	}
}