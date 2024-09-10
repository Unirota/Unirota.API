using FluentMigrator;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(202409092005)]
public class Migration202409092005 : Migration
{
    public override void Up()
    {
        Create.Table("Convites")
            .EntityBase()
            .WithColumn("Aceito").AsBoolean().NotNullable()
            .WithColumn("UsuarioId").AsInt32().NotNullable().ForeignKey("Usuarios", "Id")
            .WithColumn("MotoristaId").AsInt32().NotNullable().ForeignKey("Usuarios", "Id")
            .WithColumn("GrupoId").AsInt32().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Convites");
    }
}
