using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Direct.Parser.Database.Interfaces;
using Direct.Parser.Database.Models;
using Microsoft.EntityFrameworkCore;
using Vostok.Logging.Abstractions;


namespace Direct.Parser.Database.Repositories
{
    public class SQLAdsRepository : IAdsRepository
    {
		private DirectParserContex db;
		private ILog log;

		public SQLAdsRepository(DirectParserContex _db, ILog log)
		{
			this.db = _db;
			this.log = log;
		}

		public async Task<List<Ad>> GetAdList()
		{
			return await db.Ads.ToListAsync();
		}

		public async Task AddAd(Ad newAd)
		{
			db.Ads.Add(newAd);
			await Save();
			log.Info("Ad Was add in database ID: " + newAd.Id +
				" Title: " + newAd.Title);
		}

		public async Task AddOrUpdateAd(Ad adForUpdate)
		{
			Ad ad = db.Ads.FirstOrDefault(ad => ad.Id == adForUpdate.Id);
			if (ad == null)
			{
				await AddAd(adForUpdate);
				return;
			}
			db.Ads.Update(adForUpdate);
			await Save();
			log.Warn(
				"Ad was update ID: " + adForUpdate.Id +
				" Title: " + adForUpdate.Title +
				" Old Promotion end date: " + ad.promotionEndDate +
				" New Promotion end date: " + adForUpdate.promotionEndDate);
			return;
		}

		public async Task DeleteAd(Ad adForDelete)
		{
			Ad ad = db.Ads.FirstOrDefault(ad => ad.Id == adForDelete.Id);
			if (ad == null)
			{
				log.Warn("Ad for delete was not foung ID:" + adForDelete.Id);
				return;
			}
			db.Ads.Remove(ad);
			await Save();
			log.Warn(
				"Ad was remove ID: " + adForDelete.Id + 
				" Title: " + adForDelete.Title);
			return;
		}

		private async Task Save()
		{
			await db.SaveChangesAsync();
		}

	}
}
