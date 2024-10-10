using MediatR;
using Unirota.Domain.Common.Contracts;


namespace Unirota.Application.Commands.Grupos;

public class CriarGrupoCommand : IRequest<int>
{
    public string Nome { get; set; } = string.Empty;
    public int PassageiroLimite { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string ImagemUrl { get; set; } = string.Empty;
    public DateTime HoraInicio { get; set; }
    public bool Ativo { get; set; }
    public string Destino { get; set; }
}
