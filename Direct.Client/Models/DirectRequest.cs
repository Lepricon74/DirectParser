using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Direct.Client.Models
{
    public record DirectRequest<SelectionCriteriaType>(string method, Params<SelectionCriteriaType> @params);

    public record Params<SelectionCriteriaType>(
        SelectionCriteriaType SelectionCriteria,
        string[] FieldNames
    );
}
