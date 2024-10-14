using Ardalis.Specification;
using Unirota.Domain.Entities.Grupos;
using Unirota.Domain.Entities.Mensagens;

namespace Unirota.Application.Specifications.Mensagens;

public class ConsultarMensagemPorIdSpec : Specification<Mensagem>
{
    public ConsultarMensagemPorIdSpec(int id)
    {
        Query.Where(m => m.Id == id);
    }
}