using Unirota.Domain.Common.Contracts;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Domain.Entities.Mensagens;

public class Mensagem : BaseEntity, IAggregateRoot
{
    public string Conteudo { get; private set; }
    public int UsuarioId { get; private set; }
    public int GrupoId { get; private set; }
    
    public Grupo Grupo { get; private set; }

    
    private Mensagem(string conteudo, int usuarioId, int grupoId)
    {
        Conteudo = conteudo;
        UsuarioId = usuarioId;
        GrupoId = grupoId;
    }

    // Método de fábrica estático
    public static Mensagem Criar(string conteudo, int usuarioId, int grupoId)
    {
        // Aqui você pode adicionar validações se necessário
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new ArgumentException("O conteúdo da mensagem não pode ser vazio.");

        if (conteudo.Length > 512)
            throw new ArgumentException("O conteúdo da mensagem não pode exceder 512 caracteres.");

        return new Mensagem(conteudo, usuarioId, grupoId);
    }
}