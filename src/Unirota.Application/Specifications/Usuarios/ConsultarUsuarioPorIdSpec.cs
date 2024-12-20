﻿using Ardalis.Specification;

namespace Unirota.Application.Specifications.Usuarios;

public class ConsultarUsuarioPorIdSpec : Specification<Domain.Entities.Usuarios.Usuario>
{
    public ConsultarUsuarioPorIdSpec(int id)
    {
        Query
            .Include(x => x.GruposComoMotorista)
            .Include(x => x.Endereco)
            .Where(usuario => usuario.Id == id);
    }
}
