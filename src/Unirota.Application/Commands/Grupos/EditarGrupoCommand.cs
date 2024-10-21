using MediatR;
using Unirota.Application.ViewModels.Grupo;

namespace Unirota.Application.Commands.Grupos;

public class EditarGrupoCommand : IRequest<GrupoViewModel>

{
    public int Id { get; set; }
    public int PassageiroLimite { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string ImagemUrl { get; set; } = string.Empty;
    public DateTime HoraInicio { get; set; }
    public bool Ativo { get; set; }
}
