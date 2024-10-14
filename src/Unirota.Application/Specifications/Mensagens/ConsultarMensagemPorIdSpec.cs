using Ardalis.Specification;
using Unirota.Domain.Entities.Grupos;

namespace Unirota.Application.Specifications.Mensagens;

public class ConsultarMensagemPorIdSpec : Specification<Mensagem>, ISingleResultSpecification
{
    public ConsultarMensagemPorIdSpec(int id)
    {
        Query.Where(m => m.Id == id);
    }
}