﻿using Microsoft.AspNetCore.Mvc;
using Direct.Parser.Database;
using Direct.Parser.Database.Repositories;
using Direct.Parser.Database.Interfaces;
using Vostok.Logging.Abstractions;
using System.Threading.Tasks;

namespace Direct.Web.Controllers
{
    [Route("api/[controller]")]
    public class AdsController : Controller
    {
        private IAdsRepository adsRepository;
        private ILog log;
        public AdsController(DirectParserContex _dbContext, ILog _log)
        {
            adsRepository = new SQLAdsRepository(_dbContext, _log);
            log = _log;
        }
        [Route("all")]
        public async Task<JsonResult> GetAllAds()
        {
            log.Info("GET-ALL-ADS-REQUEST"
                     + " HostAddress: " + (Request.HttpContext.Connection.RemoteIpAddress) + ","
                     + " UserAgent: "+ (Request.Headers["User-Agent"])+","
                     + " Platform: " + Request.Headers["sec-ch-ua-platform"]);
            var adsList = await adsRepository.GetAdList();
            var result = new JsonResult(adsList);
            return result;
        }
    }
}
