namespace Unirota.API.Responses;

public class MultiStatusResponse<T>
{
    public MultiStatusResponse(T data)
    {

    }

    public T Data { get; private set; }

    public List<string> Notifications { get; set; } = new List<string>();
}
