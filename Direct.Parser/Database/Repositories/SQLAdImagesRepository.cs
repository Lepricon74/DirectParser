using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Direct.Parser.Database.Interfaces;
using Direct.Parser.Database.Models;
using Microsoft.EntityFrameworkCore;
using Vostok.Logging.Abstractions;

namespace Direct.Parser.Database.Repositories
{
	public class SQLAdImagesRepository : IAdImagesRepository
	{
		private DirectParserContex _db;
		private ILog _log;

		public SQLAdImagesRepository(DirectParserContex db, ILog log)
		{
			_db = db;
			_log = log;
		}
		public async Task<List<AdImage>> GetAdImagesList()
		{
			try
			{
				_log.Info("Trying get all adImages from database");
				return await _db.AdImages.ToListAsync();
			}
			catch (Exception ex)
			{
				_log.Error("Get all adImages from database fail: " + ex.Message);
			}

			return null;
		}

		public async Task AddAdImage(AdImage adImage)
		{
			try
			{
				_db.AdImages.Add(adImage);
				await Save();
				_log.Info(
					"AdImage Was add in database Hash: " + adImage.ImageHash +";");
			}
			catch (InvalidOperationException ex) 
			{
				_log.Error("AdImage Hash: " + adImage.ImageHash + "database add fail: " + ex.Message);
			}
		}

		public async Task DeleteAdImage(AdImage adImageForDelete)
		{
			var adImage = await _db.AdImages.FirstOrDefaultAsync(image => image.ImageHash == adImageForDelete.ImageHash);
			if (adImage == null)
			{
				_log.Warn("AdImage for delete was not found Hash:" + adImageForDelete.ImageHash);
				return;
			}
			try
			{
				_db.AdImages.Remove(adImage);
				await Save();
				_log.Info("AdImage was remove Hash: " + adImageForDelete.ImageHash);
			}
			catch (Exception ex)
			{
				_log.Error("AdImages Hash: " + adImageForDelete.ImageHash + " database remove exception " + ex.Message);
			}
		}

		private async Task Save()
		{
			await _db.SaveChangesAsync();
		}
	}
}