namespace Direct.Client.Models.Errors
{
    public record RequestError(string error_string, string error_detail, string request_id, int error_code);
}
