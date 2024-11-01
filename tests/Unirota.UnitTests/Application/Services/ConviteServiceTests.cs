using Moq;
using Unirota.Application.Commands.Convites;
using Unirota.Application.Persistence;
using Unirota.Application.Services;
using Unirota.Application.Services.Convites;
using Unirota.Application.Specifications.Convites;
using Unirota.Domain.Entities.Covites;
using Xunit;

namespace Unirota.UnitTests.Application.Services;

public class ConviteServiceTests
{
    private readonly Mock<IRepository<Convite>> _repository = new();
    private readonly Mock<IServiceContext> _serviceContext = new();
    private readonly ConviteService _service;

    public ConviteServiceTests()
    {
        _service = new(_repository.Object, _serviceContext.Object);
    }

    [Fact(DisplayName = "Deve criar um novo convite com sucesso")]
    public async Task Criar_DeveCriarConvite_QuandoNaoExisteConvitePendente()
    {
        // Arrange
        var dto = new CriarConviteCommand { UsuarioId = 1, MotoristaId = 2, GrupoId = 3 };
        _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), CancellationToken.None))
            .ReturnsAsync(null as Convite);

        // Act
        var result = await _service.Criar(dto);

        // Assert
        _repository.Verify(repo => repo.AddAsync(It.IsAny<Convite>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar 0 e adicionar erro quando já existe um convite pendente")]
    public async Task Criar_DeveRetornarZero_QuandoExisteConvitePendente()
    {
        // Arrange
        var dto = new CriarConviteCommand { UsuarioId = 1, MotoristaId = 2, GrupoId = 3 };
        var conviteExistente = new Convite(dto.UsuarioId, dto.MotoristaId, dto.GrupoId);

        _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), CancellationToken.None))
            .ReturnsAsync(conviteExistente);

        // Act
        var result = await _service.Criar(dto);

        // Assert
        Assert.Equal(0, result); // Verifica se 0 é retornado
        _serviceContext.Verify(sc => sc.AddError("Já existe um convite pendente para este usuário e motorista."), Times.Once);
        _repository.Verify(repo => repo.AddAsync(It.IsAny<Convite>(), CancellationToken.None), Times.Never);
    }

    [Fact(DisplayName = "Deve aceitar o convite com sucesso")]
    public async Task Aceitar_DeveAceitarConvite_QuandoConviteExistente()
    {
        // Arrange
        var dto = new AceitarConviteCommand { Id = 1 };
        var convite = new Convite(1, 2, 3);

        _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), CancellationToken.None))
            .ReturnsAsync(convite);

        // Act
        var result = await _service.Aceitar(dto);

        // Assert
        Assert.True(result);
        _repository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Once); 
        _serviceContext.Verify(sc => sc.AddError(It.IsAny<string>()), Times.Never);
    }

    [Fact(DisplayName = "Deve adicionar erro e retornar falso quando convite não encontrado")]
    public async Task Aceitar_DeveRetornarFalso_QuandoConviteNaoEncontrado()
    {
        // Arrange
        var dto = new AceitarConviteCommand { Id = 1 };
        _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarConvitePorIdSpec>(), CancellationToken.None))
            .ReturnsAsync(null as Convite);

        // Act
        var result = await _service.Aceitar(dto);

        // Assert
        Assert.False(result);
        _serviceContext.Verify(sc => sc.AddError("Convite não encontrado"), Times.Once);
        _repository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Never);
    }

    [Fact(DisplayName = "Deve cancelar o convite com sucesso")]
    public async Task Cancelar_DeveCancelarConvite_QuandoConviteExistente()
    {
        // Arrange
        var convite = new Convite(1, 2, 3);
        _repository.Setup(repo => repo.DeleteAsync(convite, default)).Returns(Task.CompletedTask);

        // Act
        await _service.Cancelar(convite);

        // Assert
        _repository.Verify(repo => repo.DeleteAsync(convite, default), Times.Once);
    }

    [Fact(DisplayName = "Deve chamar DeleteAsync com o convite correto")]
    public async Task Cancelar_DeveChamarDeleteAsync_ParaConviteCorreto()
    {
        // Arrange
        var convite = new Convite(1, 2, 3);
        _repository.Setup(repo => repo.DeleteAsync(convite, default)).Returns(Task.CompletedTask);

        // Act
        await _service.Cancelar(convite);

        // Assert
        _repository.Verify(repo => repo.DeleteAsync(convite, default), Times.Once);
    }
}
