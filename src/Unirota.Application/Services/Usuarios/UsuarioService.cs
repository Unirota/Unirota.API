using Mapster;
using Unirota.Application.Commands.Usuarios;
using Unirota.Application.Common.Interfaces;
using Unirota.Application.Persistence;
using Unirota.Application.Specifications.Usuarios;
using Unirota.Application.ViewModels.Usuarios;
using Unirota.Domain.Entities.Usuarios;
using Unirota.Shared;

namespace Unirota.Application.Services.Usuarios;

public class UsuarioService : IUsuarioService
{
    private readonly IRepository<Usuario> _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IServiceContext _serviceContext;

    public UsuarioService(IRepository<Usuario> repository,
                          ICurrentUser currentUser,
                          IServiceContext serviceContext)
    {
        _repository = repository;
        _currentUser = currentUser;
        _serviceContext = serviceContext;
    }

    public string CriptografarSenha(string senha)
    {
        return HashHelper.Hash(senha);
    }

    public bool ValidarSenha(string senha, string senhaCriptografada)
    {
        return HashHelper.ValidarSenha(senha, senhaCriptografada);
    }
    
    public string ValidarCpf(string cpf)
    {
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Distinct().Count() == 1)
            return "CPF inválido: todos os dígitos são iguais";

        int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

        int remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        string digit = remainder.ToString();
        tempCpf = tempCpf + digit;
        sum = 0;

        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        digit = digit + remainder.ToString();

        return cpf.EndsWith(digit) ? "Ok" : "CPF inválido";
    }

    public async Task<UsuarioViewModel?> Editar(EditarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var current = _currentUser.GetUserId();

        if (current != request.Id)
        {
            _serviceContext.AddError("Usuário não pode editar outro usuário.");
            return null;
        }

        var usuarioDb = await _repository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(request.Id), cancellationToken);

        if(usuarioDb == null)
        {
            _serviceContext.AddError("Usuário não cadastrado.");
            return null;
        }

        if(!string.IsNullOrEmpty(request.Nome))
        {
            usuarioDb
                .AlterarNome(request.Nome);
        }

        if (request.DataNascimento.HasValue)
        {
            usuarioDb
                .AlterarDataNascimento(request.DataNascimento.Value);
        }

        if (!string.IsNullOrEmpty(request.Senha))
        {
            var senhaCriptografada = CriptografarSenha(request.Senha);

            usuarioDb
                .AlterarSenha(senhaCriptografada);
        }

        if(!string.IsNullOrEmpty(request.ImagemUrl))
        {
            usuarioDb
                .AlterarImagem(request.ImagemUrl);
        }

        usuarioDb.Endereco?.AlterarLogradouro(request.Endereco.Logradouro)
                           .AlterarBairro(request.Endereco.Bairro)
                           .AlterarNumero(request.Endereco.Numero)
                           .AlterarComplemento(request.Endereco.Complemento)
                           .AlterarCEP(request.Endereco.CEP)
                           .AlterarCidade(request.Endereco.Cidade);

        await _repository.SaveChangesAsync(cancellationToken);

        return usuarioDb.Adapt<UsuarioViewModel>();
    }
    
    

    public async Task<UsuarioViewModel?> ConsultarPorId(int usuarioId, CancellationToken cancellationToken)
    {
        var usuario = await _repository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(usuarioId), cancellationToken);
        
        if (usuario is null)
        {
            _serviceContext.AddError("Usuário não existente");
            return null;
        }

        return usuario.Adapt<UsuarioViewModel>();
    }

    public async Task<bool> VerificarUsuarioExiste(int usuarioId)
    {
        var usuario = await _repository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(usuarioId));
        return usuario != null;
    }

    public async Task<bool> CadastrarMotorista(string habilitacao)
    {
        try
        {
            var current = _currentUser.GetUserId();
            var usuario = await _repository.FirstOrDefaultAsync(new ConsultarUsuarioPorIdSpec(current));
            if(usuario is null)
                return false;
            usuario = usuario.AlterarHabilitacao(habilitacao);
            await _repository.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
