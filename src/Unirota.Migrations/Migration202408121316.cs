using FluentMigrator;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(202408121316)]
public class Migration202408121316 : Migration
{
    public override void Up()
    {
        Create.Table("Usuarios")
            .EntityBase()
            .WithColumn("Nome").AsString().NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Habilitacao").AsString().Unique().Nullable()
            .WithColumn("Motorista").AsBoolean().WithDefaultValue(false).NotNullable()
            .WithColumn("Senha").AsString().NotNullable()
            .WithColumn("CPF").AsString().Unique().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("usuarios");
    }
}
