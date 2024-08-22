using FluentMigrator;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(202408121316)]
public class Migration202408121316 : Migration
{
    public override void Up()
    {
        Create.Table("usuarios")
            .EntityBase()
            .WithColumn("nome").AsString().NotNullable()
            .WithColumn("email").AsString().NotNullable()
            .WithColumn("habilitacao").AsString().Unique().Nullable()
            .WithColumn("motorista").AsBoolean().WithDefaultValue(false).NotNullable()
            .WithColumn("senha").AsString().NotNullable()
            .WithColumn("cpf").AsString().Unique().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("usuarios");
    }
}
