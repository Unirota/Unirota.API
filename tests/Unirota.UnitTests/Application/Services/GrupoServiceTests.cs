using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unirota.Application.Commands.Grupos;
using Unirota.Application.Persistence;
using Unirota.Application.Queries.Grupo;
using Unirota.Application.Services.Grupos;
using Unirota.Application.Services;
using Unirota.Domain.Entities.Grupos;
using Unirota.UnitTests.Builder;
using Xunit;
using FluentAssertions;
using Unirota.Application.Specifications.Grupos;

namespace Unirota.UnitTests.Application.Services;

public class GrupoServiceTests
{
    private readonly Mock<IRepository<Grupo>> _repositoryMock;
    private readonly Mock<IServiceContext> _serviceContextMock;
    private readonly GrupoService _grupoService;

    public GrupoServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Grupo>>();
        _serviceContextMock = new Mock<IServiceContext>();
        _grupoService = new GrupoService(_repositoryMock.Object, _serviceContextMock.Object);
    }

    [Fact(DisplayName = "Deve criar grupo com sucesso e retornar ID")]
    public async Task Criar_ShouldCreateGroupAndReturnId()
    {
        // Arrange
        var dto = new CriarGrupoCommand
        {
            Nome = "Grupo Teste",
            PassageiroLimite = 5,
            HoraInicio = DateTime.Now,
            Destino = "Destino Teste",
            Descricao = "Descrição Teste",
            ImagemUrl = "ImagemUrl"
        };
        var motoristaId = 1;
        var grupo = new Grupo(dto.Nome, dto.PassageiroLimite, dto.HoraInicio, motoristaId, dto.Destino);


        // Act
        var grupoId = await _grupoService.Criar(dto, motoristaId);

        // Assert
        grupoId.Should().Be(grupo.Id);
        _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Grupo>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar erro ao obter grupo inexistente")]
    public async Task ObterPorId_ShouldReturnError_WhenGroupNotFound()
    {
        // Arrange
        var request = new ConsultarGrupoPorIdQuery { Id = 1 };

        _repositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Grupo);

        // Act
        var result = await _grupoService.ObterPorId(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _serviceContextMock.Verify(sc => sc.AddError("Grupo não encontrado"), Times.Once);
    }

    [Fact(DisplayName = "Deve verificar se o usuário pertence ao grupo")]
    public async Task VerificarUsuarioPertenceAoGrupo_ShouldReturnTrue_WhenUserIsMember()
    {
        // Arrange
        var grupo = new GrupoBuilder().WithMotoristaId(1).WithPassageiro(2).Build();
        _repositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), CancellationToken.None))
            .ReturnsAsync(grupo);

        // Act
        var result = await _grupoService.VerificarUsuarioPertenceAoGrupo(2, grupo.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "Deve verificar se o grupo atingiu o limite de passageiros")]
    public async Task VerificarGrupoAtingiuLimiteUsuarios_ShouldReturnTrue_WhenPassengerLimitReached()
    {
        // Arrange
        var grupo = new GrupoBuilder().WithLimite(2).WithPassageiro(2).WithPassageiro(3).Build();

        _repositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ConsultarGrupoPorIdSpec>(), CancellationToken.None))
            .ReturnsAsync(grupo);

        // Act
        var result = await _grupoService.VerificarGrupoAtingiuLimiteUsuarios(grupo.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "Deve deletar grupo com sucesso")]
    public async Task Deletar_ShouldDeleteGroup()
    {
        // Arrange
        var grupo = new GrupoBuilder().Build();
        var dto = new DeletarGrupoCommand { Id = grupo.Id };

        // Act
        var result = await _grupoService.Deletar(dto, grupo);

        // Assert
        result.Should().BeTrue();
        _repositoryMock.Verify(repo => repo.DeleteAsync(grupo, CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar os grupos do usuário")]
    public async Task ObterPorUsuarioId_ShouldReturnUserGroups()
    {
        // Arrange
        var usuarioId = 1;
        var gruposComoPassageiro = new List<Grupo> { new GrupoBuilder().Build() };
        var gruposComoMotorista = new List<Grupo> { new GrupoBuilder().Build() };

        _repositoryMock.Setup(repo => repo.ListAsync(It.IsAny<ConsultarGrupoComoPassageiroSpec>(), CancellationToken.None))
            .ReturnsAsync(gruposComoPassageiro);
        _repositoryMock.Setup(repo => repo.ListAsync(It.IsAny<ConsultarGrupoComoMotoristaSpec>(), CancellationToken.None))
            .ReturnsAsync(gruposComoMotorista);

        // Act
        var result = await _grupoService.ObterPorUsuarioId(usuarioId);

        // Assert
        result.Should().HaveCount(2);
        result.Select(g => g.Id).Should().Contain(new[] { gruposComoPassageiro[0].Id, gruposComoMotorista[0].Id });
    }
}
