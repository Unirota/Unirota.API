using Ardalis.Specification;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;

namespace Unirota.Application.Specifications.SolicitacaoEntrada;

public class ConsultarSolicitacaoEntradaPorIdSpec : Specification<SolicitacaoDeEntrada>
{
    public ConsultarSolicitacaoEntradaPorIdSpec(int solicitacaoId)
    {
        Query
            .Include(solicitacao => solicitacao.Grupo)
            .Where(solicitacao => solicitacao.Id == solicitacaoId);
    }
}
