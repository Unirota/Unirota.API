using FluentMigrator;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(202410161948)]
public class Migration202410161000 : Migration
{
    public override void Up()
    {
        Create.Table("Avaliacoes")
            .EntityBase()
            .WithColumn("Nota").AsInt32().NotNullable()
            .WithColumn("UsuarioId").AsInt32().NotNullable().ForeignKey("Usuarios", "Id")
            .WithColumn("CorridaId").AsInt32().NotNullable().ForeignKey("Corridas", "Id");
    }

    public override void Down()
    {
        Delete.Table("Avaliacoes");
    }
}