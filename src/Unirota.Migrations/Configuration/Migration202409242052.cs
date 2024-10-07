using FluentMigrator;

namespace Unirota.Migrations.Configuration;

[Migration(202409242052)]

public class Migration202409242052: Migration
{
    public override void Up()
    {
        Create.Table("SolicitacaoDeEntrada")
            .EntityBase()
            .WithColumn("UsuarioId").AsInt32().NotNullable().ForeignKey("Usuarios", "Id")
            .WithColumn("GrupoId").AsInt32().NotNullable().ForeignKey("Grupos", "Id")
            .WithColumn("Aceito").AsBoolean().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("SolicitacaoDeEntrada");
    }
}