using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;
using Unirota.Domain.Entities.Usuarios;
using Unirota.Domain.Entities.UsuariosGrupos;

namespace Unirota.Domain.Entities.Grupos;

public class Mensagem : BaseEntity, IAggregateRoot
{
    public const int TamanhoMaximoMensagem = 512;
    public string Conteudo { get; private set; }
    public int UsuarioID { get; protected set; }
    public int GrupoId { get; private set; }
    
    public Usuario Usuario { get; private set; }
    public Grupo Grupo { get; private set; }
    protected Mensagem() { }

    private Mensagem(string conteudo, int usuarioId, int grupoId)
    {
        SetConteudo(Conteudo);
        UsuarioID = usuarioId;
        GrupoId = grupoId;
    }
    public static Mensagem Criar(string conteudo, int usuarioId, int grupoId)
    {
        return new Mensagem(conteudo, usuarioId, grupoId);
    }

    private void SetConteudo(string conteudo)
    {
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new ArgumentException("O conteúdo da mensagem não pode ser vazio.");

        if (conteudo.Length > TamanhoMaximoMensagem)
            throw new ArgumentException($"O tamanho máximo da mensagem é de {TamanhoMaximoMensagem} caracteres.");

        Conteudo = conteudo;
    }
}