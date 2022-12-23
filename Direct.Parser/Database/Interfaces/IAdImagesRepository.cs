using System.Collections.Generic;
using System.Threading.Tasks;
using Direct.Parser.Database.Models;

namespace Direct.Parser.Database.Interfaces
{
	public interface IAdImagesRepository
	{
		public Task<List<AdImage>> GetAdImagesList();
		public Task AddAdImage(AdImage adImage);
		public Task DeleteAdImage(AdImage adImageForDelete);
	}
}