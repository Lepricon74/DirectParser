namespace Direct.Client.Models
{
    public record DirectRequest<ParamsType>(string method, ParamsType @params);
}
