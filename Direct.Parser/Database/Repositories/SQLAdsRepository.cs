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
			try
			{
				log.Info("Trying get all ads from database");
				return await db.Ads.ToListAsync();
			}
			catch (Exception ex)
			{
				log.Error("Get all ads from database fail: " + ex.Message);
			}
			return null;
		}

		public async Task AddAd(Ad newAd)
		{
			try
			{
				db.Ads.Add(newAd);
				await Save();
				log.Info(
					"Ad Was add in database ID: " + newAd.Id +
					"; Title: " + newAd.Title);
			}
			catch (InvalidOperationException ex) 
			{
				log.Error("Ad ID: " + newAd.Id + "database add fail: " + ex.Message);
			}
			
		}

		public async Task AddOrUpdateAd(Ad adForUpdate)
		{
			Ad ad = db.Ads.FirstOrDefault(ad => ad.Id == adForUpdate.Id);
			if (ad == null)
			{
				await AddAd(adForUpdate);
				return;
			}
			if(CompareAds(ad, adForUpdate))
			{
				log.Info(
					"The information in the advertisement has not changed. No update required. Ad ID: " + ad.Id +
					"; Title: " + ad.Title +
					"; AdText: " + ad.TextAd);
				return;
			}
			ad.Title = adForUpdate.Title;
			ad.TextAd = adForUpdate.TextAd;
			ad.Status = adForUpdate.Status;
			var oldPromotionEndDate = ad.promotionEndDate;
			ad.promotionEndDate = adForUpdate.promotionEndDate;
			try
			{
				db.Ads.Update(ad);
				await Save();
				log.Info(
					"Ad was update ID: " + ad.Id +
					"; Title: " + ad.Title +
					"; AdText: " + ad.TextAd +
					"; Old Promotion end date: " + string.Join(",", oldPromotionEndDate) +
					"; New Promotion end date: " + string.Join(",", ad.promotionEndDate));
			}
			catch (InvalidOperationException ex)
			{
				log.Error("Ad ID: " + adForUpdate.Id + " database update fail: " + ex.Message );
			}		
		}

		private bool CompareAds(Ad ad1, Ad ad2)
		{
			var arrayEqual = Enumerable.SequenceEqual(ad1.promotionEndDate, ad2.promotionEndDate);
			if (ad1.Title == ad2.Title && ad1.TextAd == ad2.TextAd && ad1.Status == ad2.Status && arrayEqual)
				return true;
			return false;
		}

		public async Task DeleteAd(Ad adForDelete)
		{
			Ad ad = db.Ads.FirstOrDefault(ad => ad.Id == adForDelete.Id);
			if (ad == null)
			{
				log.Warn("Ad for delete was not foung ID:" + adForDelete.Id);
				return;
			}
			try
			{
				db.Ads.Remove(ad);
				await Save();
				log.Info(
					"Ad was remove ID: " + adForDelete.Id +
					"; Title: " + adForDelete.Title);
			}
			catch (InvalidOperationException ex)
			{
				log.Error("Ad ID: " + adForDelete.Id + " database remove exception " + ex.Message);
			}
		}

		private async Task Save()
		{
			await db.SaveChangesAsync();
		}

	}
}
