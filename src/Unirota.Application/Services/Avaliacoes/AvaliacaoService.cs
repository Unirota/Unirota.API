using Mapster;
using Unirota.Application.Commands.Avaliacoes;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Avaliacao;
using Unirota.Application.Services.Corrida;
using Unirota.Application.Services.Usuarios;
using Unirota.Application.Specifications.Avaliacoes;
using Unirota.Application.ViewModels.Avaliacoes;
using Unirota.Domain.Entities.Avaliacoes;

namespace Unirota.Application.Services.Avaliacoes;

internal class AvaliacaoService : IAvaliacaoService
{
    private readonly IRepository<Avaliacao> _avaliacaoRepository;
    private readonly ICorridaService _corridaService;
    private readonly IUsuarioService _usuarioService;
    private readonly IServiceContext _serviceContext;

    public AvaliacaoService(
        IRepository<Avaliacao> avaliacaoRepository, 
        ICorridaService corridaService,
        IUsuarioService usuarioService,
        IServiceContext serviceContext)
    {
        _avaliacaoRepository = avaliacaoRepository;
        _corridaService = corridaService;
        _usuarioService = usuarioService;
        _serviceContext = serviceContext;
    }

    public async Task<int> Criar(CriarAvaliacaoCommand command, int usuarioId)
    {
        var corrida = await _corridaService.ObterPorId(command.CorridaId);
        
        if (corrida is null) 
        {
            _serviceContext.AddError("Corrida não encontrada");
            return 0;
        }
    
        if (!await _usuarioService.VerificarUsuarioExiste(usuarioId))
        {
            _serviceContext.AddError("Usuário não encontrado");
            return 0;
        }
    
        try
        {
            var avaliacao = new Avaliacao(command.Nota, usuarioId, command.CorridaId);
            await _avaliacaoRepository.AddAsync(avaliacao);
            return avaliacao.Id;
        }
        catch (ArgumentException ex)
        {
            _serviceContext.AddError(ex.Message);
            return 0;
        }
    }

    public async Task<ICollection<ListarAvaliacoesViewModel>> ObterPorCorridaId(int corridaId)
    {
        var avaliacoes = await _avaliacaoRepository.ListAsync(new ConsultarAvaliacoesPorCorridaSpec(corridaId));
        return avaliacoes.Adapt<List<ListarAvaliacoesViewModel>>();
    }
}