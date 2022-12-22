using System.Threading.Tasks;
using Direct.Parser.Database;
using Direct.Parser.Database.Interfaces;
using Direct.Parser.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using Vostok.Logging.Abstractions;

namespace Direct.Web.Controllers
{
	[Route("api/[controller]")]
	public class AdImagesController : Controller
	{
		private IAdImagesRepository _adImagesRepository;
		private ILog _log;

		public AdImagesController(DirectParserContex directParserContext, ILog log)
		{
			_adImagesRepository = new SQLAdImagesRepository(directParserContext, log);
			_log = log;
		}

		[Route("all")]
		public async Task<JsonResult> GetAllAds()
		{
			_log.Info("GET-ALL-ADS-REQUEST"
					 + " HostAddress: " + (Request.HttpContext.Connection.RemoteIpAddress) + ","
					 + " UserAgent: "+ (Request.Headers["User-Agent"])+","
					 + " Platform: " + Request.Headers["sec-ch-ua-platform"]);
			var adsList = await _adImagesRepository.GetAdImagesList();
			var result = new JsonResult(adsList);
			return result;
		}
	}
}