using Ardalis.Specification;
using Unirota.Domain.Entities.SolicitacoesDeEntrada;

namespace Unirota.Application.Specifications.SolicitacaoEntrada;

public class ObterSolicitacaoPorUsuarioIdSpec : Specification<SolicitacaoDeEntrada>
{
    public ObterSolicitacaoPorUsuarioIdSpec(int usuarioId)
    {
        Query.Include(x => x.Grupo)
                .ThenInclude(x => x.Passageiros)
            .Include(x => x.Grupo.Mensagens)
            .Include(x => x.Grupo.Motorista)
            .Where(x => x.UsuarioId == usuarioId && x.Aceito == false);
    }
}
