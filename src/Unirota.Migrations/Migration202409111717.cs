using FluentMigrator;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(202409111717)]
public class Migration202409111717 : Migration
{
    public override void Up()
    {
        Create.Table("UsuariosGrupo")
            .EntityBase()
            .WithColumn("UsuarioId").AsInt32().NotNullable().ForeignKey("Usuarios", "Id")
            .WithColumn("GrupoId").AsInt32().NotNullable().ForeignKey("Grupos", "Id");
    }

    public override void Down()
    {
        Delete.Table("UsuariosGrupo");
    }
}
