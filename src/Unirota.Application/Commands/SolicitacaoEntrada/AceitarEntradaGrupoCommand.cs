using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Unirota.Application.Commands.SolicitacaoEntrada;

public class AceitarEntradaGrupoCommand : IRequest<bool>
{
    [FromRoute]
    public int Id { get; set; }

    [JsonIgnore]
    public string? ContextId {get;set;}
}
