using FluentMigrator;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(202410101935)]
public class Migration202410101935 : Migration
{
    public override void Up()
    {
        Create.Table("Mensagens")
            .EntityBase()
            .WithColumn("Conteudo").AsString(512).NotNullable()
            .WithColumn("UsuarioId").AsInt32().NotNullable().ForeignKey("Usuarios", "Id")
            .WithColumn("GrupoId").AsInt32().NotNullable().ForeignKey("Grupos", "Id");
    }

    public override void Down()
    {
        Delete.Table("Mensagens");
    }
}