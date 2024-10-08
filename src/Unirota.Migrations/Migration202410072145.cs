using FluentMigrator;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(202410082145)]
public class Migration202410082145 : Migration
{
    public override void Up()
    {
        Create.Table("Enderecos")
            .EntityBase()
            .WithColumn("UsuarioId").AsInt32().ForeignKey("Usuarios", "Id")
            .WithColumn("CEP").AsString().NotNullable()
            .WithColumn("Logradouro").AsString().NotNullable()
            .WithColumn("Numero").AsInt32().NotNullable()
            .WithColumn("Cidade").AsString().NotNullable()
            .WithColumn("UF").AsString().NotNullable()
            .WithColumn("Bairro").AsString().NotNullable()
            .WithColumn("Complemento").AsString().Nullable();
    }

    public override void Down()
    {
        Delete.Table("Enderecos");
    }
}
