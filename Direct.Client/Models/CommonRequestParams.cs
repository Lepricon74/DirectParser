namespace Direct.Client.Models
{
    public record CommonRequestParams<SelectionCriteriaType>(
        SelectionCriteriaType SelectionCriteria,
        string[] FieldNames
    );
}
