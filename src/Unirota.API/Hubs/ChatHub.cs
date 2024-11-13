using Microsoft.AspNetCore.SignalR;

namespace Unirota.API.Hubs;

public class ChatHub : Hub
{
    public async Task EnviarMensagemAoGrupo(int grupoId, int usuarioId, string mensagem)
    {
        await Clients.Group(grupoId.ToString()).SendAsync("ReceiveMessage", usuarioId, mensagem);
    }

    public async Task TestConnection()
    {
        await Clients.Caller.SendAsync("ReceiveMessage", "Connection successful");
    }

    public async Task EntrarGrupo(int grupoId)
    {
        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, grupoId.ToString());
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in EntrarGrupo: {ex.Message}");
            throw;
        }
    }
}
