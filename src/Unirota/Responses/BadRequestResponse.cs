namespace Unirota.API.Responses;

public class BadRequestResponse
{
    public string Instance { get; set; } = string.Empty;
    public BadRequestErrorResponse Error { get; set; }
    public List<BadRequestErrorResponse> Errors { get; set; } = new List<BadRequestErrorResponse>();
}
