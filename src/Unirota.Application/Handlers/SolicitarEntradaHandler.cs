using MediatR;
using Microsoft.EntityFrameworkCore;
using Unirota.Application.Interfaces;
using Unirota.Domain.Entities;

namespace Unirota.Application.Handlers.Grupos
{
    public class SolicitarEntradaGrupoHandler : IRequestHandler<SolicitarEntradaGrupoCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public SolicitarEntradaGrupoHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(SolicitarEntradaGrupoCommand request, CancellationToken cancellationToken)
        {
            var grupo = await _context.Grupos
                .Include(g => g.Membros)
                .FirstOrDefaultAsync(g => g.Id == request.GrupoId, cancellationToken);

            if (grupo == null || !grupo.Ativo)
            {
                throw new Exception("Grupo não encontrado ou inativo.");
            }

            if (grupo.Membros.Any(m => m.UsuarioId == request.UsuarioId))
            {
                throw new Exception("O usuário já pertence a este grupo.");
            }

            if (grupo.Membros.Count >= grupo.LimiteUsuarios)
            {
                throw new Exception("O grupo atingiu o limite de usuários.");
            }

            var solicitacao = new SolicitacaoEntrada
            {
                UsuarioId = request.UsuarioId,
                GrupoId = request.GrupoId,
                Aceito = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.SolicitacoesEntrada.Add(solicitacao);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}