using Unirota.Domain.Common.Contracts;

namespace Unirota.Domain.Entities.Grupos;

public class Mensagem : BaseEntity, IAggregateRoot
{
    public string Conteudo { get; private set; }
    public int UsuarioId { get; private set; }
    public int GrupoId { get; private set; }

    // Construtor privado
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