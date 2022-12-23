namespace Direct.Client.Models.AdImages
{
	public record AdImagesRequestSelectionCriteria(
		string[] AdImageHashes,
		string Associated
	);
}