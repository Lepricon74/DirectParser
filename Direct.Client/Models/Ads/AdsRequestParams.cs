namespace Direct.Client.Models.Ads
{
    public record AdsRequestParams<SelectionCriteriaType>(
        SelectionCriteriaType SelectionCriteria,
        string[] FieldNames,
        string[] TextAdFieldNames
    ) : CommonRequestParams<SelectionCriteriaType>(SelectionCriteria, FieldNames);
}
