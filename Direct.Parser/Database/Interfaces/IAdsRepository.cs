using System.Collections.Generic;
using System.Threading.Tasks;
using Direct.Parser.Database.Models;

namespace Direct.Parser.Database.Interfaces
{
    public interface IAdsRepository
    {
		public Task<List<Ad>> GetAdList();
		public Task AddAd(Ad newAd);
		public Task AddOrUpdateAd(Ad adForUpdate);
		public Task DeleteAd(Ad adForDelete);
	}
}
