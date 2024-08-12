namespace Unirota.API.Responses;

public class BadRequestErrorResponse
{
    public string Type { get; set; }
    public string Error { get; set; }
    public string Detail { get; set; }
    public string Property { get; set; }
}
